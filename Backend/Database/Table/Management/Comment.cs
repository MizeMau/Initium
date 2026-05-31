using Backend.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Management
{
    public class Comment
    {
        [Table("Comment", Schema = "management")]
        public class Model : IDeleteable
        {
            [Key]
            public long ManagementCommentID { get; set; }
            public DateTime Created {  get; set; }
            public DateTime? Deleted { get; set; }
            /// <summary>
            /// nvarchar(MAX)
            /// </summary>
            public string Message { get; set; } = string.Empty;
            public long ManagementTaskID { get; set; }  
            public long BackendUserID_CreatedBy { get; set; }
        }

        public class Service : Service<Model>
        {

        }
    }
}
