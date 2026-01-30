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
            /// <summary>
            /// nvarchar(64)
            /// </summary>
            public string Name { get; set; } = string.Empty;
            public long BackendUserID_CreatedBy { get; set; }
        }

        public class DTO
        {
            public class Project : Model
            {
                public List<Section.DTO.Section> Sections { get; set; } = new();
            }
        }

        public class Service : Table.Service<Model>
        {
            public List<Model> GetAllWithAccess(long backendUserID, HttpRequest? request = null)
            {
                return GetQuery(request)
                    .Where(project => project.BackendUserID_CreatedBy == backendUserID)
                    .ToList();
            }
        }
    }
}
