using Backend.Util;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Ability
    {
        [Table("Ability", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonAbilityID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            [MaxLength(64)]
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public long? PokemonTypeID { get; set; }
            public byte? Multiplier { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
