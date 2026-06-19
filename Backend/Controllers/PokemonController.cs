using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : Controller
    {
        [HttpGet("")]
        public IActionResult Pokemon_GET()
        {
            return Ok();
        }

        [HttpGet("Location/{pokemonLocationID}")]
        public IActionResult Location_PokemonLocationID_Get(long pokemonLocationID, long pokemonSaveID)
        {
            var pokemonLocationService = new Database.Table.Pokemon.Location.Service();
            var pokemonRouteService = new Database.View.Pokemon.Route.Service();
            var pokemonSaveService = new Database.Table.Pokemon.Save.Service();
            var pokemonSave_PokemonService = new Database.Table.Pokemon.Save_Pokemon.Service();
            var pokemonEvolutionService = new Database.Table.Pokemon.Evolution.Service();
            var pokemonEncounterService = new Database.View.Pokemon.Encounter.Service();

            var save = pokemonSaveService.GetById(pokemonSaveID);
            if (save == null)
                return NoContent();

            var dblocation = pokemonLocationService.GetById(pokemonLocationID);
            if (dblocation == null)
                return NoContent();
            var location = Util.Converter.Convert<Database.Table.Pokemon.Location.DTO.Location>(dblocation);

            var encounter = pokemonEncounterService.GetAll(pokemonLocationID, save.PokemonModeID);
            location.Encounter = Util.Converter.Convert<Database.View.Pokemon.Encounter.DTO.Encounter>(encounter);

            var caughtPokemonIDs = pokemonSave_PokemonService.GetQuery()
                .Where(w => w.PokemonSaveID == pokemonSaveID)
                .Select(s => s.PokemonPokemonID)
                .ToHashSet();
            var evolutions = pokemonEvolutionService.GetAll();

            location.Encounter = location.Encounter.Select(s =>
            {
                var chain = pokemonEvolutionService.GetFullEvolutionChain(s.PokemonPokemonID, evolutions);
                s.IsCaught = chain.Any(id => caughtPokemonIDs.Contains(id));
                return s;
            }).ToList();

            location.Routes = pokemonRouteService.GetAllByLocation(pokemonLocationID);

            return Ok(location);
        }

        [HttpPost("Save/Pokemon")]
        public IActionResult Save_Pokemon_Post([FromBody] Database.Table.Pokemon.Save_Pokemon.Model model)
        {
            var pokemonSave_PokemonService = new Database.Table.Pokemon.Save_Pokemon.Service();
            var save_pokemon = pokemonSave_PokemonService.Create(model);
            return Ok(save_pokemon);
        }
        [HttpDelete("Save/Pokemon")]
        public IActionResult Save_Pokemon_Delete([FromQuery] long pokemonPokemonID, [FromQuery] long pokemonSaveID)
        {
            var pokemonSave_PokemonService = new Database.Table.Pokemon.Save_Pokemon.Service();
            var success = pokemonSave_PokemonService.Delete(pokemonSaveID, pokemonPokemonID, true);
            return Ok(success);
        }
    }
}
