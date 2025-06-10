using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Users
{
    [Table("UserDetails")]
    public class User : AuditProperties
    {
        public string Name { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public UserLoginMapping? UserLoginMappings { get; set; }
    }
}
