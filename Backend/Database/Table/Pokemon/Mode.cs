using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Mode
    {
        [Table("Mode", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonModeID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            [MaxLength(16)]
            public string Name { get; set; } = string.Empty;
        }

        public class Service : Service<Model>
        {

        }
    }
}
