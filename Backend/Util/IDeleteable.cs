namespace Backend.Util
{
    public interface IDeleteable
    {
        public DateTime Created { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
