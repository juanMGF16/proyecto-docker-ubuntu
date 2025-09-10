using Business.Services.SendEmail.Interfaces;
using Entity.Models.ParametersModule;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Business.Services.SendEmail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            return await SendEmailAsync(new List<string> { toEmail }, subject, body, isHtml);
        }

        public async Task<bool> SendEmailAsync(List<string> toEmails, string subject, string body, bool isHtml = false)
        {
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.EnableSsl = _emailSettings.EnableSsl;
                    client.UseDefaultCredentials = _emailSettings.UseDefaultCredentials;
                    client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);

                        foreach (var email in toEmails.Where(e => !string.IsNullOrWhiteSpace(e)))
                        {
                            message.To.Add(email);
                        }

                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = isHtml;

                        await client.SendMailAsync(message);

                        _logger.LogInformation($"Email sent successfully to {string.Join(", ", toEmails)}");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {string.Join(", ", toEmails)}");
                return false;
            }
        }

        public async Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body,
            byte[] attachment, string attachmentName, bool isHtml = false)
        {
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.EnableSsl = _emailSettings.EnableSsl;
                    client.UseDefaultCredentials = _emailSettings.UseDefaultCredentials;
                    client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                        message.To.Add(toEmail);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = isHtml;

                        using (var stream = new MemoryStream(attachment))
                        {
                            var attachmentFile = new Attachment(stream, attachmentName);
                            message.Attachments.Add(attachmentFile);
                        }

                        await client.SendMailAsync(message);

                        _logger.LogInformation($"Email with attachment sent successfully to {toEmail}");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email with attachment to {toEmail}");
                return false;
            }
        }
    }
}

