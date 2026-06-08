using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Encounter
    {
        public enum Type
        {
            Grass = 1,
            PokeRadar = 2,
            Surf = 3,
            OldRod = 4,
            GoodRod = 5,
            SuperRod = 6,
            RockSmash = 7,
            Cave = 8,
        }
        [Table("Encounter", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonEncounterID { get; set; }
            public long PokemonPokemonID { get; set; }
            public long PokemonLocationID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public int Type { get; set; }
            public byte Level_Low { get; set; }
            public byte Level_High { get; set; }
            public byte? Dawn { get; set; }
            public byte? Noon { get; set; }
            public byte? Dusk { get; set; }
            public long PokemonModeID { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
