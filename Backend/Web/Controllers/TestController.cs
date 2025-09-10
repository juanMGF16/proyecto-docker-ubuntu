using Data.Factory;
using Entity.Models.ParametersModule;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums.Models;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IDataFactoryGlobal _factory;

        public TestController(IDataFactoryGlobal factory)
        {
            _factory = factory;
        }

        [HttpPost("createnotification")]
        public async Task<IActionResult> CreateTestNotification([FromBody] TestNotificationRequest request)
        {
            try
            {
                var notificationData = _factory.CreateNotificationData();

                var notification = new Notification
                {
                    Title = request.Title,
                    Content = request.Content,
                    Type = TypeNotification.OperatingReminderEmail,
                    Date = DateTime.UtcNow,
                    Read = false,
                    UserId = request.UserId,
                    Active = true
                };

                var result = await notificationData.CreateAsync(notification);
                return Ok(new
                {
                    success = true,
                    message = "Notificación creada",
                    id = result.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("getnotifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            try
            {
                var notificationData = _factory.CreateNotificationData();
                var notifications = await notificationData.GetUserNotificationsAsync(userId);

                return Ok(new
                {
                    success = true,
                    count = notifications.Count(),
                    notifications
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }

    public class TestNotificationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}