namespace BA.Service.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, string ccMail = "", string bccMail = "");
    }
}
