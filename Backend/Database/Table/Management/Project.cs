using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Project
    {
        [Table("Project", Schema = "management")]
        public class Model : BaseModel
        {
            [Key]
            public long ManagementProjectID { get; set; }
            public string Name { get; set; } = string.Empty;
            public long BackendUserID_CreatedBy { get; set; }
        }

        public class DTO
        {

        }

        public class Service : Table.Service<Model>
        {
            public List<Model> GetAllWithAccess(long backendUserID)
            {
                return GetQuery()
                    .Where(project => project.BackendUserID_CreatedBy == backendUserID)
                    .ToList();
            }
        }
    }
}
