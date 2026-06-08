using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Pokemon
    {
        [Table("Pokemon", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonPokemonID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public int Number { get; set; }
            [MaxLength(64)]
            public string Name { get; set; } = string.Empty;
            public byte HP { get; set; }
            public byte ATK { get; set; }
            public byte DEF { get; set; }
            public byte SPATK { get; set; }
            public byte SPDEF { get; set; }
            public byte SPEED { get; set; }
            public byte CatchRate { get; set; }
            public long PokemonTypeID_First { get; set; }
            public long? PokemonTypeID_Second { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
