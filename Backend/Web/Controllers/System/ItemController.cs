using Business.Repository.Interfaces.Specific.System;
using Business.Services.CargaMasiva;
using Entity.DTOs.CargaMasiva;
using Entity.DTOs.ParametersModels;
using Entity.DTOs.System.Item;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.System
{
    [Route("api/[controller]/")]
    public class ItemController : BaseController<IItemBusiness>
    {
        private readonly IItemBulkService _bulkService;
        public ItemController(IItemBulkService bulkService, IItemBusiness itemBusiness, ILogger<ItemController> logger)
            : base(itemBusiness, logger)
        {
            _bulkService = bulkService;
        }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<ItemDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAll");


        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpGet("GetItemsSpecific/{id:int}")]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllItemsSpecific(int id) =>
            await TryExecuteAsync(() => _service.GetAllItemsSpecificAsync(id), "GetById");

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(ItemDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ItemDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "Createitem");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] ItemDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "Updateitem");

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, [FromQuery] DeleteType strategy = DeleteType.Logical)
        {
            return await TryExecuteAsync(() => _service.DeleteAsync(id, strategy), "DeleteItem");
        }

        [HttpPost("upload-excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadExcel([FromForm] UploadExcelDTO dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Debe subir un archivo Excel válido.");

            var result = await _bulkService.UploadExcelAsync(dto.File, dto.ZoneId);

            return Ok(result);
        }

    }
}
