namespace BA.Dtos.EmailTemplateDto
{
    public class GetEmailTemplateDetailsDto
    {
        public string EmailType { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public string EmailTableBody { get; set; } = string.Empty;
    }
}
