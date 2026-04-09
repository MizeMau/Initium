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
            public int SortNumber { get; set; }
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
            public override Model Create(Model model)
            {
                long? lastSortNumber = GetQuery()
                    .Where(w => w.ManagementProjectID == model.ManagementProjectID)
                    .OrderByDescending(o => o.SortNumber)
                    .FirstOrDefault()
                    ?.SortNumber;

                lastSortNumber ??= 0;

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
                    .OrderBy(o => o.SortNumber)
                    .ToList();
            }
            public List<Model> GetAllByProject(Project.Model project)
            {
                return GetAllByProject(project.ManagementProjectID);
            }
            public bool UpdateSortNumber(Model item, int itemIndex)
            {
                var query = GetQuery()
                    .Where(w => w.ManagementProjectID == item.ManagementProjectID)
                    .Where(w => w.ManagementSectionID != item.ManagementSectionID)
                    .OrderBy(o => o.SortNumber);

                int provinceSortNumber = itemIndex - 1 >= 0 ? query.Skip(itemIndex - 1).First().SortNumber : 0;
                long nextSortNumber = query.Skip(itemIndex).FirstOrDefault()?.SortNumber ?? int.MaxValue;
                int newSortNumber = (int)((provinceSortNumber + nextSortNumber) / 2);

                var success = UpdateProperty(item.ManagementSectionID, u => u.SortNumber, newSortNumber);

                // prevent very close numbers
                if (newSortNumber == provinceSortNumber + 1 
                    || newSortNumber == nextSortNumber - 1
                    || newSortNumber == provinceSortNumber)
                    success &= CalculateSortNumbers(item);

                return success;
            }

            public bool CalculateSortNumbers(Model model)
            {
                return CalculateSortNumbers(model.ManagementProjectID);
            }
            public bool CalculateSortNumbers(long managementProjectID)
            {
                int max = int.MaxValue;
                bool success = true;

                var ids = GetQuery()
                    .Where(w => w.ManagementProjectID == managementProjectID)
                    .OrderBy(o => o.SortNumber)
                    .Select(s => s.ManagementSectionID)
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
