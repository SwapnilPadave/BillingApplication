using BA.Entities.Customer;
using BA.Entities.NewsPaper;
using BA.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BA.Database
{
    public class BAContext(DbContextOptions context) : DbContext(context)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLoginMapping> UserLoginMappings { get; set; }
        public DbSet<NewsPaperDetails> NewsPaperDetails { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
    }
}
