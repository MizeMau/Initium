using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Field
    {
        public enum Type
        {
            Select = 1,
        }

        [Table("Field", Schema = "management")]
        public class Model : IDeleteable
        {
            [Key]
            public long ManagementFieldID { get; set; }
            public DateTime Created {  get; set; }
            public DateTime? Deleted { get; set; }
            public Type Type { get; set; }
            /// <summary>
            /// nvarchar(64)
            /// </summary>
            public string Name { get; set; } = string.Empty;
            public long ManagementSectionID { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
