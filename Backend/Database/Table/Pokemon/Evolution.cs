using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Evolution
    {
        public enum Type
        {
            Level = 1,
            Item = 2,
        }
        [Table("Evolution", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonEvolutionID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public Type Type { get; set; }
            public byte? Level { get; set; }
            public long? PokemonItemID { get; set; }
            public long PokemonPokemonID_Base { get; set; }
            public long PokemonPokemonID_Evolution { get; set; }
        }

        public class Service : Service<Model>
        {
            public HashSet<long> GetFullEvolutionChain(long pokemonPokemonID)
            {
                var evolutions = GetAll();
                return GetFullEvolutionChain(pokemonPokemonID, evolutions);
            }
            public HashSet<long> GetFullEvolutionChain(long pokemonPokemonID, List<Database.Table.Pokemon.Evolution.Model> allEvolutions)
            {
                var chain = new HashSet<long>();
                var toVisit = new Queue<long>();

                toVisit.Enqueue(pokemonPokemonID);

                while (toVisit.Count > 0)
                {
                    var current = toVisit.Dequeue();
                    if (!chain.Add(current)) // already visited, prevents infinite loops
                        continue;

                    // Walk forward — things this pokemon evolves into
                    foreach (var evo in allEvolutions.Where(e => e.PokemonPokemonID_Base == current))
                        toVisit.Enqueue(evo.PokemonPokemonID_Evolution);

                    // Walk backward — things that evolve into this pokemon (pre-evolutions)
                    foreach (var evo in allEvolutions.Where(e => e.PokemonPokemonID_Evolution == current))
                        toVisit.Enqueue(evo.PokemonPokemonID_Base);
                }

                return chain;
            }
        }
    }
}
