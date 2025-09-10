namespace Business.Services.SendEmail.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false);
        Task<bool> SendEmailAsync(List<string> toEmails, string subject, string body, bool isHtml = false);
        Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachment, string attachmentName, bool isHtml = false);
    }
}
