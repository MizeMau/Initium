using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Pokemon_Ability
    {
        [Table("Pokemon_Ability", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonPokemon_AbilityID { get; set; }
            public long PokemonPokemonID { get; set; }
            public long PokemonAbilityID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public bool IsHidden { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
