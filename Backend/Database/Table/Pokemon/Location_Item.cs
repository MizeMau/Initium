using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Location_Item
    {
        [Table("Location_Item", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonLocation_ItemID { get; set; }
            public long PokemonLocationID { get; set; }
            public long PokemonItemID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
