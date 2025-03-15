using LuanAnTotNghiep_TuanVu_TuBac.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace LuanAnTotNghiep_TuanVu_TuBac.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            var client = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, _configuration["EmailSettings:SenderName"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            await client.SendMailAsync(mailMessage);
        }
    }
}