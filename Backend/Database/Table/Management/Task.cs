using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            public int SortNumber { get; set; }
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
            public long? ManagementTaskID_Head { get; set; }
        }

        public class DTO
        {
            public class Task : Model
            {
                public List<Model> Tasks { get; set; } = new();
            }
            public class UpdateTaskSection
            {
                public long ManagementTaskID { get; set; }
                public long ManagementSectionID { get; set; }
            }
        }

        public class Service : Table.Service<Model>
        {
            public override Model Create(Model model)
            {
                long lastSortNumber = GetQuery()
                    .Where(w => w.ManagementSectionID == model.ManagementSectionID)
                    .Where(w => w.ManagementTaskID_Head == model.ManagementTaskID_Head)
                    .OrderByDescending(o => o.SortNumber)
                    .Select(s => s.SortNumber)
                    .FirstOrDefault();

                model.SortNumber = (int)((lastSortNumber + int.MaxValue) / 2);

                base.Create(model);

                if (model.SortNumber >= int.MaxValue - 1)
                    CalculateSortNumbers(model);

                return model;
            }
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

            public bool UpdateSortNumber(Model item, int itemIndex)
            {
                var query = GetQuery()
                    .Where(w => w.ManagementSectionID == item.ManagementSectionID)
                    .Where(w => w.ManagementTaskID_Head == item.ManagementTaskID_Head)
                    .Where(w => w.ManagementTaskID != item.ManagementTaskID)
                    .OrderBy(o => o.SortNumber);

                int provinceSortNumber = itemIndex - 1 >= 0 ? query.Skip(itemIndex - 1).First().SortNumber : 0;
                long nextSortNumber = query.Skip(itemIndex).FirstOrDefault()?.SortNumber ?? int.MaxValue;
                int newSortNumber = (int)((provinceSortNumber + nextSortNumber) / 2);

                var success = UpdateProperty(item.ManagementTaskID, u => u.SortNumber, newSortNumber);

                // prevent very close numbers
                if (newSortNumber == provinceSortNumber + 1 || newSortNumber == nextSortNumber - 1)
                    success &= CalculateSortNumbers(item);

                return success;
            }

            public bool CalculateSortNumbers(Model model)
            {
                return CalculateSortNumbers(model.ManagementSectionID, model.ManagementTaskID_Head);
            }
            public bool CalculateSortNumbers(long managementSectionID, long? managementTaskID_Head = null)
            {
                int max = int.MaxValue;
                bool success = true;

                var ids = GetQuery()
                    .Where(w => w.ManagementSectionID == managementSectionID)
                    .Where(w => w.ManagementTaskID_Head == managementTaskID_Head)
                    .OrderBy(o => o.SortNumber)
                    .Select(s => s.ManagementTaskID)
                    .ToList();

                int increament = max / (ids.Count() + 2);
                for (int i = 0; i < ids.Count(); i++)
                {
                    long id = ids[i];
                    success &= UpdateProperty(id, u => u.SortNumber, increament * (i + 1));
                }
                return success;
            }
        }
    }
}
