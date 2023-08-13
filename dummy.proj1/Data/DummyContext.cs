using Microsoft.EntityFrameworkCore;
using dummy.Models;
using Microsoft.Extensions.Configuration;

namespace dummy.Data
{
    public class DummyContext : DbContext
    {
        private string _connectionString;
        public DbSet<User> Users => Set<User>();
        public DbSet<TodoModel> TodoModels => Set<TodoModel>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Bank> Bank => Set<Bank>();

        public DbSet<CustomPost> CustomPosts => Set<CustomPost>();

        // public DummyContext(DbContextOptions<DummyContext> options) : base(options) { }

        public DummyContext()
        {

            ConfigurationBuilder configurationBuilder = new();
            configurationBuilder.AddJsonFile("appsettings.json");
            configurationBuilder.AddUserSecrets<Program>();
            var configuration = configurationBuilder.Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            string dbPassword = configuration["DbPassword"];

            var conStrBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
            conStrBuilder.Password = dbPassword;

            _connectionString = conStrBuilder.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Posts)
                .WithOne(e => (Models.User)e.User)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TodoModels)
                .WithOne(e => (Models.User)e.User)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<User>()
                .HasOne(e => e.Bank)
                .WithOne(e => (Models.User)e.User)
                .HasForeignKey<Bank>(e => e.UserId)
                .HasPrincipalKey<User>(e => e.Id);

            modelBuilder.Entity<Bank>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CustomPost>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}