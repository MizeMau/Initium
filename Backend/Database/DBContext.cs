using Microsoft.EntityFrameworkCore;

namespace Backend.Database
{
    public class DBContext : DbContext
    {
        public static string ConnectionString { 
            get {
                return "Server=MizeServerWindows;Database=Volgatus;User Id=Initium;Password=Initium;TrustServerCertificate=True;";
            } 
        }

        #region Backend
        public DbSet<Table.Backend.User.Model> BackendUser { get; set; }
        #endregion
        #region Management
        public DbSet<Table.Management.Project.Model> ManagementProject { get; set; }
        public DbSet<Table.Management.Section.Model> ManagementSection { get; set; }
        #endregion
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
