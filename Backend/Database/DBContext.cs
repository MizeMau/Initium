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
                return "Server=WIN-ND6EUS8O78K;Database=Volgatus;User Id=Initium;Password=Initium;TrustServerCertificate=True;";
            } 
        }

        #region Backend
        public DbSet<Table.Backend.User.Model> BackendUser { get; set; }
        #endregion

        #region Management
        public DbSet<Table.Management.Comment.Model> ManagementComment { get; set; }
        public DbSet<Table.Management.Field.Model> ManagementField { get; set; }
        public DbSet<Table.Management.History.Model> ManagementHistory { get; set; }
        public DbSet<Table.Management.Project.Model> ManagementProject { get; set; }
        public DbSet<Table.Management.Project_User.Model> ManagementProject_User { get; set; }
        public DbSet<Table.Management.Section.Model> ManagementSection { get; set; }
        public DbSet<Table.Management.Task.Model> ManagementTask { get; set; }
        public DbSet<Table.Management.Task_User.Model> ManagementTask_User { get; set; }
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
