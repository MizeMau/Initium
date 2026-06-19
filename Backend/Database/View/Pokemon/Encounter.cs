using Backend.Database.Table;
using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.View.Pokemon
{
    public class Encounter
    {

        [Table("Encounter", Schema = "pokemon")]
        public class Model
        {
            [Key]
            public long PokemonLocation_PokemonID { get; set; }
            public long PokemonPokemonID { get; set; }
            public long PokemonLocationID { get; set; }
            public long PokemonModeID { get; set; }
            [MaxLength(64)]
            public string PokemonName { get; set; } = string.Empty;
            [MaxLength(8)]
            public string TypeName_First { get; set; } = string.Empty;
            [MaxLength(8)]
            public string? TypeName_Second { get; set; }
            public byte Level_Low { get; set; }
            public byte Level_High { get; set; }
            public byte CatchRate { get; set; }
            public byte? Dawn { get; set; }
            public byte? Noon { get; set; }
            public byte? Dusk { get; set; }
            public Backend.Database.Table.Pokemon.Location_Pokemon.Type Type { get; set; }
            public int? AVGEncounterPercentage { get; set; }
            public long PokemonLocationID_Best { get; set; }
            [MaxLength(64)]
            public string PokemonLocationName_Best { get; set; } = string.Empty;
            public int? AVGEncounterPercentage_Best { get; set; }
        }

        public class DTO
        {
            public class Encounter : Model
            {
                public bool IsCaught { get; set; }
            }
        }

        public class Service : ViewService<Model>
        {
            public List<Model> GetAll(long pokemonLocationID, long pokemonModeID)
            {
                return GetQuery()
                    .Where(w => w.PokemonLocationID == pokemonLocationID)
                    .Where(w => w.PokemonModeID == pokemonModeID)
                    .ToList();
            }
        }
    }
}
