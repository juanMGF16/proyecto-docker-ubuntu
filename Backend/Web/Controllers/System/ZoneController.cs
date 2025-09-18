using Business.Repository.Interfaces.Specific.System;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Zone;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR")]
        [HttpGet("GetByIdBranch/{id:int}")]
        [ProducesResponseType(typeof(ZoneSimpleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdBranch(int id) =>
            await TryExecuteAsync(() => _service.GetZonesByBranchAsync(id), "GetByIdBranch");

<<<<<<< HEAD
        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR")]
        [HttpGet("GetZoneDetailsById/{zoneId}")]
        [ProducesResponseType(typeof(ZoneDetailsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetZoneDetails(int zoneId) =>
                await TryExecuteAsync(() => _service.GetZoneDetailsAsync(zoneId), "GetZoneDetailsById");
=======
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
>>>>>>> parent of 845d2803 (solucion de errores)

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR")]
        [HttpGet("GetInCharges/{branchId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ZoneInChargeListDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetInChargesByCompany(int branchId) =>
            await TryExecuteAsync(() => _service.GetInChargesAsync(branchId), "GetInCharges");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR, ENCARGADO_ZONA")]
        [HttpGet("GetZoneByAreaManager/{id:int}")]
        [ProducesResponseType(typeof(ZoneConsultDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetZoneByAreaManager(int id) =>
            await TryExecuteAsync(() => _service.GetZoneByAreaManagerAsync(id), "GetZoneByAreaManager");

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

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR, ENCARGADO_ZONA")]
        [HttpPatch("PartialUpdate/")]
        [ProducesResponseType(typeof(ZoneDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PartialUpdate([FromBody] ZonePartialUpdateDTO dto) =>
            await TryExecuteAsync(() => _service.PartialUpdateAsync(dto), "PartialUpdate");

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
