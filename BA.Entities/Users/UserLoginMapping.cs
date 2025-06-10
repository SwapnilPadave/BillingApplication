using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Users
{
    public class UserLoginMapping
    {
        [Key]
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
