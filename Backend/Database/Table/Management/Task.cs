using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Task
    {
        [Table("Task", Schema = "management")]
        public class Model : BaseModel
        {
            [Key]
            public long ManagementTaskID { get; set; }
            /// <summary>
            /// nvarchar(128)
            /// </summary>
            public string Name { get; set; } = string.Empty;
            /// <summary>
            /// nvarchar(MAX),
            /// saves HTML content from text field
            /// </summary>
            public string Description { get; set; } = string.Empty;
            public bool IsCompleted { get; set; }
            public bool IsMilestone { get; set; }
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }
            public long ManagementProjectID { get; set; }
            public long ManagementSectionID { get; set; }
        }

        public class DTO
        {

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
