using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using UESAN.VDI.CORE.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace UESAN.VDI.CORE.Core.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPortStr = _configuration["Smtp:Port"];
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var smtpFrom = _configuration["Smtp:From"];

            if (string.IsNullOrWhiteSpace(smtpHost) ||
                string.IsNullOrWhiteSpace(smtpPortStr) ||
                string.IsNullOrWhiteSpace(smtpUser) ||
                string.IsNullOrWhiteSpace(smtpPass) ||
                string.IsNullOrWhiteSpace(smtpFrom))
            {
                throw new InvalidOperationException("SMTP configuration is missing or incomplete.");
            }

            var smtpPort = int.Parse(smtpPortStr);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };
            var mail = new MailMessage(smtpFrom, to, subject, body);
            await client.SendMailAsync(mail);
        }
    }
}
