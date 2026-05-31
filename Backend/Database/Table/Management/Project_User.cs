using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Project_User
    {
        public enum Role
        {
            Owner = 1,
            Member = 2,
        }
        [Table("Project_User", Schema = "management")]
        public class Model : IDeleteable
        {
            [Key]
            public long ManagementProject_UserID { get; set; }
            public DateTime Created {  get; set; }
            public DateTime? Deleted { get; set; }
            public long ManagementProjectID { get; set; }
            public long BackendUserID { get; set; }
            public Role Role { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
