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
            /// <summary>
            /// varchar(64)
            /// </summary>
            public string Username { get; set; } = string.Empty;
            /// <summary>
            /// varchar(MAX)
            /// </summary>
            public string Password { get; set; } = string.Empty;
            /// <summary>
            /// varchar(32)
            /// </summary>
            public string Salt { get; set; } = string.Empty;
        }

        public class DTO
        {
            public class User : BaseModel
            {
                public long BackendUserID { get; set; }
                public string Username { get; set; } = string.Empty;
            }
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
