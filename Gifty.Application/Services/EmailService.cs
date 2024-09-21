using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gifty.Application.Services;

public class EmailService
{
        private readonly string _smtpServer = "smtp.gmail.com"; // Replace with your SMTP server
        private readonly int _smtpPort = 587; // Usually 587 for TLS
        private readonly string _smtpUser = "paulo.suljic@gmail.com"; // Your email address
        private readonly string _smtpPass = "ruuv qbuv ebqh sxxr"; // Your email password
    
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                client.EnableSsl = true; // Use SSL
    
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false // Set to true if sending HTML email
                };
                mailMessage.To.Add(to);
    
                await client.SendMailAsync(mailMessage);
            }
        }
}
