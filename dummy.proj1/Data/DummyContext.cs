using Microsoft.EntityFrameworkCore;
using dummy.Models;

namespace dummy.Data
{
    public class DummyContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<TodoModel> TodoModels => Set<TodoModel>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Bank> Bank => Set<Bank>();

        public DbSet<CustomPost> CustomPosts => Set<CustomPost>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=192.168.1.69;Port=54321;Database=dummy;Username=postgres;Password=bananas");
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