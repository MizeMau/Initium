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

        #region Pokemon
        public DbSet<Table.Pokemon.Ability.Model> PokemonAbility { get; set; }
        public DbSet<Table.Pokemon.Encounter.Model> PokemonEncounter { get; set; }
        public DbSet<Table.Pokemon.Evolution.Model> PokemonEvolution { get; set; }
        public DbSet<Table.Pokemon.Item.Model> PokemonItem { get; set; }
        public DbSet<Table.Pokemon.Location.Model> PokemonLocation { get; set; }
        public DbSet<Table.Pokemon.Location_Item.Model> PokemonLocation_Item { get; set; }
        public DbSet<Table.Pokemon.Location_Location.Model> PokemonLocation_Location { get; set; }
        public DbSet<Table.Pokemon.Mode.Model> PokemonMode { get; set; }
        public DbSet<Table.Pokemon.Move.Model> PokemonMove { get; set; }
        public DbSet<Table.Pokemon.Pokemon.Model> PokemonPokemon { get; set; }
        public DbSet<Table.Pokemon.Pokemon_Ability.Model> PokemonPokemon_Ability { get; set; }
        public DbSet<Table.Pokemon.Pokemon_Move.Model> PokemonPokemon_Move { get; set; }
        public DbSet<Table.Pokemon.Save.Model> PokemonSave { get; set; }
        public DbSet<Table.Pokemon.Save_Pokemon.Model> PokemonSave_Pokemon { get; set; }
        public DbSet<Table.Pokemon.Type.Model> PokemonType { get; set; }
        public DbSet<Table.Pokemon.Type_Type.Model> PokemonType_Type { get; set; }
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
