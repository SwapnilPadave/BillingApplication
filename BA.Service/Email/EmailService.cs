using BA.Database;
using BA.Database.Infra;
using BA.Utility.AppSettings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BA.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SmtpSettings _smtpSettings;
        private readonly SqlCommands _sqlCommands;
        public EmailService(IUnitOfWork unitOfWork
            , IOptions<SmtpSettings> smtpSettings
            , SqlCommands sqlCommands)
        {
            _unitOfWork = unitOfWork;
            _smtpSettings = smtpSettings.Value;
            _sqlCommands = sqlCommands;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, string ccMail = "", string bccMail = "")
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = _smtpSettings.Host,
                    Port = _smtpSettings.Port,
                    EnableSsl = _smtpSettings.EnableSsl,
                    Credentials = new NetworkCredential(_smtpSettings.FromEmail, _smtpSettings.Password)
                };

                using var message = new MailMessage(_smtpSettings.FromEmail, to)
                {
                    Subject = subject,
                    Body = body
                };

                await smtp.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return false;
            }
        }
    }
}
