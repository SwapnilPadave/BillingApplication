using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Users
{
    [Table("Login")]
    public class UserLoginMapping : AuditProperties
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}
