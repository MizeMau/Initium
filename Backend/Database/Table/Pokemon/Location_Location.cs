using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Location_Location
    {
        public enum Type
        {
            Up = 1,
            UpRight = 2,
            Right = 3,
            DownRight = 4,
            Down = 5,
            DownLeft = 6,
            Left = 7,
            UpLeft = 8
        }
        [Table("Location_Location", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonLocation_LocationID { get; set; }
            public long PokemonLocationID_From { get; set; }
            public long PokemonLocationID_To { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public Type Direction { get; set; }
            [MaxLength(64)]
            public string? Condition { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
