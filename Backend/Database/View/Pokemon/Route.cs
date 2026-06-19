using Backend.Database.Table;
using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.View.Pokemon
{
    public class Route
    {

        [Table("Route", Schema = "pokemon")]
        public class Model
        {
            [Key]
            public long PokemonLocation_LocationID { get; set; }
            public long PokemonLocationID_From { get; set; }
            public long PokemonLocationID_To { get; set; }
            [MaxLength(64)]
            public string Name { get; set; } = string.Empty;
            public Table.Pokemon.Location_Location.Type Direction { get; set; }
            [MaxLength(64)]
            public string? Condition { get; set; }
        }

        public class Service : ViewService<Model>
        {
            public List<Model> GetAllByLocation(long pokemonLocationID)
            {
                return GetQuery()
                    .Where(w => w.PokemonLocationID_From == pokemonLocationID)
                    .OrderBy(o => o.Direction)
                    .ToList();
            }
        }
    }
}
