using BA.Entities.Bill;
using BA.Entities.Customer;
using BA.Entities.Departments;
using BA.Entities.Email;
using BA.Entities.Employees;
using BA.Entities.GeneratedBill;
using BA.Entities.NewsPaper;
using BA.Entities.Shift;
using BA.Entities.Token;
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
        public DbSet<CustomerBillDetails> CustomerBillDetails { get; set; }
        public DbSet<JwtToken> JwtToken { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<CustomerNewsPaperBillDetail> CustomerNewsPaperBillDetails { get; set; }
        public DbSet<GeneratedNewsPaperBillDetails> GeneratedNewsPaperBillDetails { get; set; }
        public DbSet<EmailTemplates> EmailTemplates { get; set; }
        }
}
