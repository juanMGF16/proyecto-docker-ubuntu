using Business.Repository.Interfaces.Specific.System;
using Entity.DTOs.System.Zone;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.System
{
    [Route("api/[controller]/")]
    public class ZoneController : BaseController<IZoneBusiness>
    {

        public ZoneController(IZoneBusiness zoneBusiness, ILogger<ZoneController> logger)
            : base(zoneBusiness, logger) { }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<ZoneConsultDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllCategory");


        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ZoneConsultDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");


        /// <summary>
        /// Obtiene todas las zonas disponibles para un usuario específico,
        /// según el OperationalGroup asignado.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetZonesByUser(int userId)
        {
            try
            {
                var zones = await _service.GetZonesByUserAsync(userId);

                if (zones == null || !zones.Any())
                    return NotFound($"No se encontraron zonas para el usuario {userId}");

                return Ok(zones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener las zonas para el usuario {userId}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocurrió un error en el servidor");
            }
        }


        [HttpPost("Create/")]
        [ProducesResponseType(typeof(ZoneDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ZoneDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "Createitem");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(ZoneDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] ZoneDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "Updateitem");

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, [FromQuery] DeleteType strategy = DeleteType.Logical)
        {
            return await TryExecuteAsync(() => _service.DeleteAsync(id, strategy), "DeleteItem");
        }
    }
}
