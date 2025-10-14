using Microsoft.EntityFrameworkCore;

namespace Backend.Database
{
    public class DBContext : DbContext
    {
        public static string ConnectionString = "Server=MizeServerWindows;Database=Volgatus;User Id=Initium;Password=Initium;TrustServerCertificate=True;";

        public DbSet<Table.Backend.User.Model> Users { get; set; }
        public DBContext() { }
        public DBContext(DbContextOptions<DBContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
