using Microsoft.EntityFrameworkCore;
using dummy.Models;

namespace dummy.Data
{
    public class DummyContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<TodoModel> TodoModels => Set<TodoModel>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Tag> Tags => Set<Tag>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=dummy;Username=postgres;Password=bananas");
        }
    }
}