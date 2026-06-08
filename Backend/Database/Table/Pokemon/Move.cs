using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Move
    {
        [Table("Move", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonMoveID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            [MaxLength(32)]
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int Type { get; set; }
            public byte? Power { get; set; }
            public byte? Accuracy { get; set; }
            public byte PP { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
