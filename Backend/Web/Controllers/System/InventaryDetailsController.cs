using Business.Repository.Interfaces.Specific.System;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.InventaryDetail;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.System
{
    [Route("api/[controller]/")]
    public class InventaryDetailsController : BaseController<IInventaryDetailBusiness>
    {

        public InventaryDetailsController(IInventaryDetailBusiness inventaryDetailBusiness, ILogger<InventaryDetailsController> logger)
            : base(inventaryDetailBusiness, logger) { }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<InventaryDetailConsultDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllCategory");


        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(InventaryDetailConsultDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(InventaryDetailDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] InventaryDetailDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "Createitem");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(InventaryDetailDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] InventaryDetailDTO dto) =>
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
