using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Type_Type
    {
        [Table("Type_Type", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonType_TypeID { get; set; }
            public long PokemonTypeID_ATK { get; set; }
            public long PokemonTypeID_DEF { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            public byte Multiplier { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
