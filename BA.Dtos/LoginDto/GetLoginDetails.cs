namespace BA.Dtos.LoginDto
{
    public class GetLoginDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool Admin { get; set; }
    }
}
