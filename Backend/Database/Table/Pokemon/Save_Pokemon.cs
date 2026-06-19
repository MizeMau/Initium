using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Save_Pokemon
    {
        [Table("Save_Pokemon", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonSave_PokemonID { get; set; }
            public long PokemonSaveID { get; set; }
            public long PokemonPokemonID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
        }

        public class Service : Service<Model>
        {
            public bool Delete(long pokemonSaveID, long pokemonPokemonID, bool hard)
            {
                var model = GetQuery().Single(s => s.PokemonSaveID == pokemonSaveID && s.PokemonPokemonID == pokemonPokemonID);
                return Delete(model.PokemonSave_PokemonID, hard);
            }
        }
    }
}
