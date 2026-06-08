using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Util
{
    public class Pokemon
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOpts;


        #region Services
        private Database.Table.Pokemon.Pokemon.Service _pokemonPokemonService;
        private Database.Table.Pokemon.Type.Service _pokemonTypeService;
        private Database.Table.Pokemon.Ability.Service _pokemonAbilityService;
        private Database.Table.Pokemon.Pokemon_Ability.Service _pokemonPokemon_AbilityService;
        private Database.Table.Pokemon.Evolution.Service _pokemonEvolutionService;
        private Database.Table.Pokemon.Item.Service _pokemonItemService;
        #endregion

        #region DB Entries
        private List<Database.Table.Pokemon.Pokemon.Model> _pokemon = new();
        private List<Database.Table.Pokemon.Type.Model> _types = new();
        private List<Database.Table.Pokemon.Ability.Model> _abilities = new();
        private List<Database.Table.Pokemon.Pokemon_Ability.Model> _pokemon_abilities = new();
        private List<Database.Table.Pokemon.Evolution.Model> _evolutions = new();
        private List<Database.Table.Pokemon.Item.Model> _items = new();
        #endregion

        public Pokemon()
        {
            _http = new HttpClient { BaseAddress = new Uri("https://infinitefusiondex.com/_next/data/d_DTHUx1_u90d9FeJN4vB/") };
            _jsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        #region Infinite Dex
        /// <summary>
        /// This fills the Tables 'Pokemon', 'Ability', 'Pokemon_Ability', 'Evolution' and 'Item'
        /// </summary>
        /// <returns>isSuccess</returns>
        public async Task<bool> FillPokemon()
        {
            bool success = true;

            #region Services
            _pokemonPokemonService = new();
            _pokemonTypeService = new();
            _pokemonAbilityService = new();
            _pokemonPokemon_AbilityService = new();
            _pokemonEvolutionService = new();
            _pokemonItemService = new();
            #endregion

            #region DB Entries
            _pokemon = _pokemonPokemonService.GetAll();
            _types = _pokemonTypeService.GetAll();
            _abilities = _pokemonAbilityService.GetAll();
            _pokemon_abilities = _pokemonPokemon_AbilityService.GetAll();
            _evolutions = _pokemonEvolutionService.GetAll();
            _items = _pokemonItemService.GetAll();
            #endregion


            var pokemons = await FetchJsonAsync<FusionDexAPIPokedex>("pokedex.json");

            if (pokemons == null)
                return false;

            foreach (var pokemon in pokemons.pageProps.pokemon.Skip(420))
            {
                Console.WriteLine($"Saving Data for {pokemon.pokemon_id} : {pokemon.canonical_url}");
                var pokePage = await FetchJsonAsync<FusionDexAPIPokemonPageProps>($"details/{pokemon.canonical_url}.json");
                if (pokePage == null) continue;
                var poke = pokePage.pageProps.pokemon.First();


                var dbPokemon = _pokemon.FirstOrDefault(f => f.Name.ToLower() == pokemon.real_name.ToLower());
                if (dbPokemon == null)
                {
                    var firstType = _types.First(f => f.Name.ToLower() == poke.type1.ToLower());
                    Database.Table.Pokemon.Type.Model? secondType = null;
                    if (poke.type2 != null)
                    {
                        secondType = _types.First(f => f.Name.ToLower() == poke.type2.ToLower());
                    }
                    var pokemonToCreate = new Database.Table.Pokemon.Pokemon.Model()
                    {
                        Number = poke.pokemon_id,
                        Name = poke.real_name,
                        HP = (byte)poke.stats.hp,
                        ATK = (byte)poke.stats.attack,
                        DEF = (byte)poke.stats.defense,
                        SPATK = (byte)poke.stats.special_attack,
                        SPDEF = (byte)poke.stats.special_defense,
                        SPEED = (byte)poke.stats.speed,
                        CatchRate = (byte)poke.catch_rate,
                        PokemonTypeID_First = firstType.PokemonTypeID,
                        PokemonTypeID_Second = secondType?.PokemonTypeID,
                    };
                    dbPokemon = _pokemonPokemonService.Create(pokemonToCreate);
                    _pokemon.Add(dbPokemon);
                }

                poke.abilities.regular.ForEach(async (f) => await SavaAbility(f, dbPokemon.PokemonPokemonID, false));
                if (poke.abilities.hidden == null)
                    continue;
                poke.abilities.hidden.ForEach(async (f) => await SavaAbility(f, dbPokemon.PokemonPokemonID, true));

                if (pokePage.pageProps.evolution_chains == null)
                    continue;
                pokePage.pageProps.evolution_chains.ForEach(async (f) => await SaveEvolution(f));
            }

            return success;
        }

        private async Task SavaAbility(FusionDexAPIPokemonAbility ability, long pokemonPokemonID, bool isHidden)
        {
            var dbAbility = _abilities.FirstOrDefault(f => f.Name.ToLower() == ability.ability_name.ToLower());
            if (dbAbility == null)
            {
                var abilityToCreate = new Database.Table.Pokemon.Ability.Model()
                {
                    Name = ability.ability_name,
                    Description = ability.effect,
                };
                dbAbility = _pokemonAbilityService.Create(abilityToCreate);
                _abilities.Add(dbAbility);
            }

            var dbPokemon_Ability = _pokemon_abilities.FirstOrDefault(f => f.PokemonPokemonID == pokemonPokemonID && f.PokemonAbilityID == dbAbility.PokemonAbilityID);
            if (dbPokemon_Ability == null)
            {
                var pokemon_abilityToCreate = new Database.Table.Pokemon.Pokemon_Ability.Model()
                {
                    PokemonPokemonID = pokemonPokemonID,
                    PokemonAbilityID = dbAbility.PokemonAbilityID,
                    IsHidden = isHidden
                };
                dbPokemon_Ability = _pokemonPokemon_AbilityService.Create(pokemon_abilityToCreate);
                _pokemon_abilities.Add(dbPokemon_Ability);
            }
        }
        private async Task SaveEvolution(FusionDexAPIPokemonEvolutionChain chain)
        {
            var pokemonPokemon_Base = _pokemon.FirstOrDefault(f => f.Name.ToLower() == chain.pokemon_object.real_name.ToLower());
            if (pokemonPokemon_Base == null) 
                return;
            long pokemonPokemonID_Base = pokemonPokemon_Base.PokemonPokemonID;

            foreach(var evolution in chain.evolutions)
            {
                var pokemonPokemon_Evolution = _pokemon.FirstOrDefault(f => f.Name.ToLower() == evolution.pokemon_object.real_name.ToLower());
                if (pokemonPokemon_Evolution == null)
                    continue;
                long pokemonPokemonID_Evolution = pokemonPokemon_Evolution.PokemonPokemonID;

                foreach (var method in evolution.methods)
                {
                    byte? level = null;
                    long? pokemonItemID = null;
                    Database.Table.Pokemon.Evolution.Type type = 0;

                    switch (method.method.ToLower())
                    {
                        case "level":
                            type = Database.Table.Pokemon.Evolution.Type.Level;
                            level = (byte)method.level;
                            break;
                        case "item":
                            type = Database.Table.Pokemon.Evolution.Type.Item;
                            pokemonItemID = await SaveItem(method.item);
                            break;
                    }


                    var dbEvolution = _evolutions.FirstOrDefault(f => f.PokemonPokemonID_Base == pokemonPokemonID_Base 
                                                                      && f.PokemonPokemonID_Evolution == pokemonPokemonID_Evolution
                                                                      && f.Type == type);
                    if (dbEvolution == null)
                    {
                        var evolutionToCreate = new Database.Table.Pokemon.Evolution.Model()
                        {
                            PokemonPokemonID_Base = pokemonPokemonID_Base,
                            PokemonPokemonID_Evolution = pokemonPokemonID_Evolution,
                            Type = type,
                            Level = level,
                            PokemonItemID = pokemonItemID,
                        };
                        dbEvolution = _pokemonEvolutionService.Create(evolutionToCreate);
                        _evolutions.Add(dbEvolution);
                    }
                }
                await SaveEvolution(evolution);
            }
        }
        private async Task<long> SaveItem(string item)
        {
            var dbItem = _items.FirstOrDefault(f => f.Name.ToLower() == item.ToLower());
            if (dbItem == null)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new Exception("Empty Item Name");
                }
                var itemToCreate = new Database.Table.Pokemon.Item.Model()
                {
                    Name = char.ToUpper(item[0]) + item.Substring(1).ToLower(),
                };
                dbItem = _pokemonItemService.Create(itemToCreate);
                _items.Add(dbItem);
            }
            return dbItem.PokemonItemID;
        }

        #region Requests
        private DateTime _lastRequest = DateTime.MinValue;
        private const int MIN_MS_BETWEEN_REQUESTS = 2000; // 1 req/sec
        private async Task RateLimitAsync()
        {
            var elapsed = (DateTime.UtcNow - _lastRequest).TotalMilliseconds;
            if (elapsed < MIN_MS_BETWEEN_REQUESTS)
                await Task.Delay((int)(MIN_MS_BETWEEN_REQUESTS - elapsed));
            _lastRequest = DateTime.UtcNow;
        }
        private async Task<T?> FetchJsonAsync<T>(string relativeOrAbsolute) where T : class
        {
            await RateLimitAsync();
            try
            {
                string url = relativeOrAbsolute.StartsWith("http")
                    ? relativeOrAbsolute
                    : relativeOrAbsolute;

                using var resp = relativeOrAbsolute.StartsWith("http")
                    ? await _http.GetAsync(relativeOrAbsolute)
                    : await _http.GetAsync(relativeOrAbsolute);

                if (!resp.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"  [HTTP {(int)resp.StatusCode}] {relativeOrAbsolute}");
                    return null;
                }

                using var stream = await resp.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOpts);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"  [ERR] {relativeOrAbsolute}: {ex.Message}");
                return null;
            }
        }
        // Pokedex list
        public record FusionDexAPIPokedex(
            [property: JsonPropertyName("pageProps")] FusionDexAPIPokedexPokemonList pageProps);
        public record FusionDexAPIPokedexPokemonList(
            [property: JsonPropertyName("pokemon")] List<FusionDexAPIPokedexPokemon> pokemon);
        public record FusionDexAPIPokedexPokemon(
            [property: JsonPropertyName("pokemon_id")] int pokemon_id,
            [property: JsonPropertyName("real_name")] string real_name,
            [property: JsonPropertyName("canonical_url")] string canonical_url);

        // Single Pokemon
        public record FusionDexAPIPokemonPageProps(
            [property: JsonPropertyName("pageProps")] FusionDexAPIPokemonPokemonList pageProps);
        public record FusionDexAPIPokemonPokemonList(
            [property: JsonPropertyName("pokemon")] List<FusionDexAPIPokemonPokemon> pokemon,
            [property: JsonPropertyName("evolution_chains")] List<FusionDexAPIPokemonEvolutionChain> evolution_chains);
        public record FusionDexAPIPokemonPokemon(
            [property: JsonPropertyName("pokemon_id")] int pokemon_id,
            [property: JsonPropertyName("real_name")] string real_name,
            [property: JsonPropertyName("creature_type")] string creature_type,
            [property: JsonPropertyName("catch_rate")] int catch_rate,
            [property: JsonPropertyName("type1")] string type1,
            [property: JsonPropertyName("type2")] string type2,
            [property: JsonPropertyName("stats")] FusionDexAPIPokemonStats stats,
            [property: JsonPropertyName("abilities")] FusionDexAPIPokemonAbilities abilities);
        public record FusionDexAPIPokemonStats(
            [property: JsonPropertyName("hp")] int hp,
            [property: JsonPropertyName("attack")] int attack,
            [property: JsonPropertyName("defense")] int defense,
            [property: JsonPropertyName("special_attack")] int special_attack,
            [property: JsonPropertyName("special_defense")] int special_defense,
            [property: JsonPropertyName("speed")] int speed);
        public record FusionDexAPIPokemonAbilities(
            [property: JsonPropertyName("regular")] List<FusionDexAPIPokemonAbility> regular,
            [property: JsonPropertyName("hidden")] List<FusionDexAPIPokemonAbility> hidden);
        public record FusionDexAPIPokemonAbility(
            [property: JsonPropertyName("ability_name")] string ability_name,
            [property: JsonPropertyName("effect")] string effect);

        public record FusionDexAPIPokemonEvolutionChain(
            [property: JsonPropertyName("evolution")] List<FusionDexAPIPokemonEvolutionChain> evolutions,
            [property: JsonPropertyName("pokemon_object")] FusionDexAPIPokemonPokemonObject pokemon_object,
            [property: JsonPropertyName("methods")] List<FusionDexAPIPokemonEvolutionMethode> methods);
        public record FusionDexAPIPokemonPokemonObject(
            [property: JsonPropertyName("real_name")] string real_name);
        public record FusionDexAPIPokemonEvolutionMethode(
            [property: JsonPropertyName("method")] string method,
            [property: JsonPropertyName("level")] int? level,
            [property: JsonPropertyName("item")] string item);
        #endregion
        #endregion
    }
}
