using Microsoft.EntityFrameworkCore;

namespace Backend.Database
{
    public class DBContext : DbContext
    {
        /// <summary>
        /// Still needs to be put in the environment variables
        /// </summary>
        public static string ConnectionString { 
            get {
                return "Host=WIN-ND6EUS8O78K;Port=5431;Username=Initium;Password=Initium;Database=postgres";
            } 
        }

        #region Backend
        public DbSet<Table.Backend.User.Model> BackendUser { get; set; }
        #endregion

        #region Management
        public DbSet<Table.Management.Project.Model> ManagementProject { get; set; }
        public DbSet<Table.Management.Section.Model> ManagementSection { get; set; }
        public DbSet<Table.Management.Task.Model> ManagementTask { get; set; }
        #endregion

        public DBContext() { }
        public DBContext(DbContextOptions<DBContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }
    }
}
