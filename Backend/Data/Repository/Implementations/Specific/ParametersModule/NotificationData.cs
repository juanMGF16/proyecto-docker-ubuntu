using Data.Repository.Interfaces.Specific.ParametersModule;
using Entity.Context;
using Entity.Models.ParametersModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.ParametersModule
{
    public class NotificationData : GenericData<Notification>, INotificationData
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public NotificationData(AppDbContext context, ILogger<Notification> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Notification>> GetAllAsync()
        {
            try
            {
                return await _context.Notification
                    .Include(n => n.User)
                    .Where(n => n.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }
        public override async Task<Notification?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Notification
                    .Include(n => n.User)
                    .FirstOrDefaultAsync(n => n.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener los datos por id: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Notification
                    .Where(n => n.UserId == userId && n.Active)
                    .OrderByDescending(n => n.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting notifications for user {userId}");
                throw;
            }
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            try
            {
                return await _context.Notification
                    .CountAsync(n => n.UserId == userId && n.Active && !n.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting unread count for user {userId}");
                throw;
            }
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notification.FindAsync(notificationId);
                if (notification == null) return false;

                notification.Read = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {notificationId} as read");
                return false;
            }
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            try
            {
                var unreadNotifications = await _context.Notification
                    .Where(n => n.UserId == userId && n.Active && !n.Read)
                    .ToListAsync();

                foreach (var notification in unreadNotifications)
                {
                    notification.Read = true;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking all notifications as read for user {userId}");
                return false;
            }
        }
    }
}
