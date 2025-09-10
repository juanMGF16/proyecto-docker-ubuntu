using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Business.Repository.Implementations.Specific.ParametersModule;
using Business.Repository.Interfaces.Specific.ParametersModule;
using Entity.DTOs.ParametersModels.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    [Route("api/[controller]")]
    public class NotificationController : BaseController<INotificationBusiness>
    {
        public NotificationController(INotificationBusiness userBusiness, ILogger<NotificationController> logger)
            : base(userBusiness, logger) { }

        [HttpGet("GetAll/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<NotificationDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllNotifications");

        [HttpGet("GetAllJWT/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<NotificationDTO>), 200)]
        public async Task<IActionResult> GetAllJWT()
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var role = roleClaim?.Value;

            if (string.Equals(role, "SM_ACTION", StringComparison.OrdinalIgnoreCase))
            {
                return await TryExecuteAsync(async () =>
                {
                    if (_service is NotificationBusiness ubGeneral)
                    {
                        return await ubGeneral.GetAllTotalNotificationsAsync();
                    }
                    throw new ValidationException("Funcionalidad no disponible para este tipo de negocio.");
                }, "GetAllTotalNotifications");
            }

            return await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllNotifications");
        }

        [HttpGet("GetById/{id:int}")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(NotificationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpPost("Create/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(NotificationOptionsDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] NotificationOptionsDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "CreateNotification");
        }

        [HttpPut("Update/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(NotificationOptionsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] NotificationOptionsDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "UpdateNotification");

        [HttpDelete("Delete/{id:int}")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, [FromQuery] DeleteType strategy = DeleteType.Logical)
        {
            return await TryExecuteAsync(() => _service.DeleteAsync(id, strategy), "DeleteRole");
        }
    }
}
