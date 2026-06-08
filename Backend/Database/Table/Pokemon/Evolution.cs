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

        }
    }
}
