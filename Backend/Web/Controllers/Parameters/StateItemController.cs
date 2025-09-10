using Business.Repository.Interfaces.Specific.System;
using Entity.DTOs.System;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.Parameters
{
    [Route("api/[controller]/")]
    public class StateItemController : BaseController<IStateItemBusiness>
    {

        public StateItemController(IStateItemBusiness stateItemBusiness, ILogger<StateItemController> logger)
            : base(stateItemBusiness, logger) { }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<StateItemDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllStateItem");


        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(StateItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(StateItemDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] StateItemDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "CreateStateItem");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(StateItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] StateItemDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "UpdateForm");

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, [FromQuery] DeleteType strategy = DeleteType.Logical)
        {
            return await TryExecuteAsync(() => _service.DeleteAsync(id, strategy), "DeleteCategory");
        }
    }
}
