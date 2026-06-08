using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Pokemon_Move
    {
        [Table("Pokemon_Move", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonPokemon_MoveID { get; set; }
            public long PokemonPokemonID { get; set; }
            public long PokemonMove { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public int Type { get; set; }
            public byte? Level { get; set; }
            public byte? Tutor { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
