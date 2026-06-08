using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Location_Location
    {
        [Table("Location_Location", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonLocation_LocationID { get; set; }
            public long PokemonLocationID_From { get; set; }
            public long PokemonLocationID_To { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public int Direction { get; set; }
            [MaxLength(64)]
            public string Condition { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
