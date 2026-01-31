using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Section
    {
        [Table("Section", Schema = "management")]
        public class Model : BaseModel
        {
            [Key]
            public long ManagementSectionID { get; set; }
            /// <summary>
            /// nvarchar(64)
            /// </summary>
            public string Name { get; set; } = string.Empty;
            public long ManagementProjectID { get; set; }
        }

        public class DTO
        {
            public class Section : Model
            {
                public List<Task.DTO.Task> Tasks { get; set; } = new();
            }
        }

        public class Service : Table.Service<Model>
        {
            public List<Model> GetAllByProject(long managementProjectID)
            {
                return GetQuery()
                    .Where(w => w.ManagementProjectID == managementProjectID)
                    .ToList();
            }
            public List<Model> GetAllByProject(Project.Model project)
            {
                return GetAllByProject(project.ManagementProjectID);
            }
        }
    }
}
