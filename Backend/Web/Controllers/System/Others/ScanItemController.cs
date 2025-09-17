using Business.Services.ScanInventary;
using Entity.DTOs.ScanItem;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.System.Others
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScanItemController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public ScanItemController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("scan")]
        public async Task<IActionResult> ScanItem([FromBody] ScanItemRequestDTO request)
        {
            var result = await _inventoryService.ScanItemAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
