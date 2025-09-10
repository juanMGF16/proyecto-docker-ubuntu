using Entity.DTOs.ParametersModels.Email;
using Entity.DTOs.ParametersModels.Notification;

namespace Business.Repository.Interfaces.Specific.ParametersModule
{
    public interface INotificationBusiness : IGenericBusiness<NotificationDTO, NotificationOptionsDTO>
    {
        // General
        Task<IEnumerable<NotificationDTO>> GetAllTotalNotificationsAsync();

        //Specific
        Task<bool> SendEmailNotificationAsync(EmailRequestDTO emailRequest);
        Task<bool> SendBulkEmailNotificationAsync(EmailRequestDTO emailRequest);
        Task LogNotificationAsync(int userId, string title, string content, string type);
    }

}