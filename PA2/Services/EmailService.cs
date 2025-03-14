using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PA2.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "your-email@gmail.com";
        private readonly string _smtpPassword = "your-app-password";

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage(_smtpUsername, to, subject, body);
                await client.SendMailAsync(message);
            }
        }
    }
}
