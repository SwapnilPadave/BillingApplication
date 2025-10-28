namespace BA.Service.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, string ccMail = "", string bccMail = "");
        Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, byte[]? attachmentBytes = null, string attachmentFileName = "", string ccMail = "", string bccMail = "");
    }
}
