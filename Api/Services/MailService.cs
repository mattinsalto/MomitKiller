using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MomitKiller.Api.Services
{
    public class MailService:IMailService
    {
        private EmailSettings _emailSettings;

        public MailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string subject, string message)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.FromEmail)
                };
                mail.To.Add(new MailAddress(_emailSettings.ToEmail));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Passwd);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error has ocurred sending the email", ex);
            }
        }
    }
}
