using Entity.Models.ParametersModule;

namespace Data.Repository.Interfaces.Specific.ParametersModule
{
    public interface INotificationData : IGenericData<Notification> {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
    }
}