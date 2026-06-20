using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Database.Table.Pokemon
{
    public class Location
    {
        [Table("Location", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonLocationID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            [MaxLength(64)]
            public string Name { get; set; } = string.Empty;
        }

        public class DTO
        {
            public class Location : Model
            {
                public List<View.Pokemon.Encounter.Model> Encounter { get; set; } = new();
                public List<View.Pokemon.Route.Model> Routes { get; set; } = new();
            }
        }

        public class Service : Service<Model>
        {

        }
    }
}
