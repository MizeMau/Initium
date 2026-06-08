using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Pokemon
{
    public class Save
    {
        [Table("Save", Schema = "pokemon")]
        public class Model : IDeleteable
        {
            [Key]
            public long PokemonSaveID { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
            [MaxLength(64)]
            public string Name { get; set; } = string.Empty;
            public long BackendUserID { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
