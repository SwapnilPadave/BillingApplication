using BA.Database;
using BA.Database.Infra;
using BA.Utility.AppSettings;
using Microsoft.Extensions.Options;
using System.IO;
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

        public async Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, byte[]? attachmentBytes = null, string attachmentFileName = "", string ccMail = "", string bccMail = "")
        {
            try
            {
                using var smtp = new SmtpClient
                {
                    Host = _smtpSettings.Host,
                    Port = _smtpSettings.Port,
                    EnableSsl = _smtpSettings.EnableSsl,
                    Credentials = new NetworkCredential(_smtpSettings.FromEmail, _smtpSettings.Password)
                };

                using var message = new MailMessage()
                {
                    From = new MailAddress(_smtpSettings.FromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(to);

                if (!string.IsNullOrEmpty(ccMail))
                    message.CC.Add(ccMail);

                if (!string.IsNullOrEmpty(bccMail))
                    message.Bcc.Add(bccMail);

                if (attachmentBytes != null && !string.IsNullOrEmpty(attachmentFileName))
                {
                    var attachmentStream = new MemoryStream(attachmentBytes);
                    attachmentStream.Position = 0;
                    var attachment = new Attachment(attachmentStream, attachmentFileName, "application/pdf");

                    //attachment.ContentDisposition.FileName = attachmentFileName;
                    //attachment.ContentDisposition.Inline = false;

                    attachment.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;
                    message.Attachments.Add(attachment);
                }
                message.Headers.Add("Content-Type", "application/pdf");

                await smtp.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return false;
            }
        }

        //    public async Task<bool> SendEmailWithAttachmentAsync(
        //string to,
        //string subject,
        //string body,
        //byte[]? attachmentBytes = null,
        //string attachmentFileName = "",
        //string ccMail = "",
        //string bccMail = "")
        //    {
        //        try
        //        {
        //            using var smtp = new SmtpClient
        //            {
        //                Host = _smtpSettings.Host,
        //                Port = _smtpSettings.Port,
        //                EnableSsl = _smtpSettings.EnableSsl,
        //                Credentials = new NetworkCredential(_smtpSettings.FromEmail, _smtpSettings.Password)
        //            };

        //            using var message = new MailMessage
        //            {
        //                From = new MailAddress(_smtpSettings.FromEmail),
        //                Subject = subject,
        //                Body = body,
        //                IsBodyHtml = true
        //            };

        //            message.To.Add(to);

        //            if (!string.IsNullOrEmpty(ccMail))
        //                message.CC.Add(ccMail);

        //            if (!string.IsNullOrEmpty(bccMail))
        //                message.Bcc.Add(bccMail);

        //            if (attachmentBytes != null && !string.IsNullOrWhiteSpace(attachmentFileName))
        //            {
        //                var stream = new MemoryStream(attachmentBytes);

        //                var attachment = new Attachment(stream, attachmentFileName, "application/pdf");

        //                // ✅ Forces Gmail to *preview* instead of download
        //                attachment.ContentDisposition.Inline = true;
        //                attachment.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;

        //                // ✅ Helps Gmail properly detect PDF
        //                attachment.ContentType = new System.Net.Mime.ContentType("application/pdf");

        //                message.Attachments.Add(attachment);
        //            }

        //            // ✅ Helps some mail servers recognize inline preview
        //            message.Headers.Add("Content-Type", "application/pdf");

        //            await smtp.SendMailAsync(message);

        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            await _sqlCommands.ExceptionLogToDatabase(ex);
        //            return false;
        //        }
        //    }

    }
}
