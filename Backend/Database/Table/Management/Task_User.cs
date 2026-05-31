using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Task_User
    {
        public enum Role
        {
            Assignee = 1,
        }
        [Table("Task_User", Schema = "management")]
        public class Model : IDeleteable
        {
            [Key]
            public long ManagementTask_UserID { get; set; }
            public DateTime Created {  get; set; }
            public DateTime? Deleted { get; set; }
            public long ManagementTaskID { get; set; }
            public long BackendUserID { get; set; }
            public Role Role { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
