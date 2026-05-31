using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class History
    {
        public enum Type
        {
            Updated = 1,
        }

        [Table("History", Schema = "management")]
        public class Model : IDeleteable
        {
            [Key]
            public long ManagementHistoryID { get; set; }
            public DateTime Created {  get; set; }
            public DateTime? Deleted { get; set; }
            public Type Type { get; set; }
            public long ManagementTaskID { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
