namespace Backend.Database
{
    public class BaseModel : IDeleteable
    {
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
    }
    public interface IDeleteable
    {
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
