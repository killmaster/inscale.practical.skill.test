using Microsoft.EntityFrameworkCore;
using dummy.Models;

namespace dummy.Data
{
    public class DummyContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<TodoModel> TodoModels => Set<TodoModel>();
        public DbSet<Post> Posts => Set<Post>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=stickers;Port=54321;Database=dummy;Username=postgres;Password=bananas");
        }
    }
}