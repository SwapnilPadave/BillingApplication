using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Token
{
    [Table("Token")]
    public class JwtToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpireAt { get; set; }
        public bool IsActive { get; set; }
    }
}
