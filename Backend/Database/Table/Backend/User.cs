using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Backend
{
    public class User
    {
        [Table("User", Schema = "backend")]
        public class Model : BaseModel
        {
            [Key]
            public long BackendUserID { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Salt { get; set; } = string.Empty;
        }

        public class DTO
        {
            public DTO(Model model)
            {
                BackendUserID = model.BackendUserID;
                Created = model.Created;
                Username = model.Username;
            }
            public long BackendUserID { get; set; }
            public DateTime Created { get; set; }
            public string Username { get; set; } = string.Empty;
        }

        public class Service : Table.Service<Model>
        {
            public Model? GetByUsername(string username)
            {
                return GetQuery().SingleOrDefault(s => s.Username == username);
            }
        }
    }
}
