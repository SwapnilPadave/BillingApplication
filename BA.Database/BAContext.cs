using BA.Entities.Bill;
using BA.Entities.Country_State_City;
using BA.Entities.Customer;
using BA.Entities.Email;
using BA.Entities.GeneratedBill;
using BA.Entities.JobApp_Departments;
using BA.Entities.JobApp_Employees;
using BA.Entities.JobApp_PositionRole;
using BA.Entities.JobApp_Shifts;
using BA.Entities.NewsPaper;
using BA.Entities.Shift;
using BA.Entities.Student;
using BA.Entities.Token;
using BA.Entities.Users;
using BA.Entities.VideoStream;
using Microsoft.EntityFrameworkCore;

namespace BA.Database
{
    public class BAContext(DbContextOptions context) : DbContext(context)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Video>().HasMany(v => v.Formats).WithOne(f => f.Video).HasForeignKey(f => f.VideoId);
            modelBuilder.Entity<Video>().HasMany(v => v.Comments).WithOne(c => c.Video).HasForeignKey(c => c.VideoId);

            ////To generate unique sequential IDs for Student entity (Identity Sequence in SQL Server)
            modelBuilder.HasSequence<int>("StudentHiLoSeq", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(10);

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(s => s.Id).UseHiLo("StudentHiLoSeq", "dbo");
            });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLoginMapping> UserLoginMappings { get; set; }
        public DbSet<NewsPaperDetails> NewsPaperDetails { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<CustomerBillDetails> CustomerBillDetails { get; set; }
        public DbSet<JwtToken> JwtToken { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<CustomerNewsPaperBillDetail> CustomerNewsPaperBillDetails { get; set; }
        public DbSet<GeneratedNewsPaperBillDetails> GeneratedNewsPaperBillDetails { get; set; }
        public DbSet<EmailTemplates> EmailTemplates { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoFormat> VideoFormats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<CountryMaster> CountryMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
        public DbSet<CityMaster> CityMaster { get; set; }


        // Job Application Related DbSets
        public DbSet<JobApp_EmployeesDetails> JobApp_EmployeesDetails { get; set; }
        public DbSet<JobApp_DepartmentDetailMaster> JobApp_DepartmentDetailMaster { get; set; }
        public DbSet<JobApp_PositionRoleDetailMaster> JobApp_PositionRoleDetailMaster { get; set; }
        public DbSet<JobApp_ShiftDetailsMaster> JobApp_ShiftDetailsMaster { get; set; }
    }
}
