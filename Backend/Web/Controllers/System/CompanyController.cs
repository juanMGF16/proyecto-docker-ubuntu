using Business.Repository.Interfaces.Specific.System;
using Entity.DTOs.System.Company;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.System
{
    [Route("api/[controller]/")]
    public class CompanyController : BaseController<ICompanyBusiness>
    {

        public CompanyController(ICompanyBusiness companyBusiness, ILogger<CompanyController> logger)
            : base(companyBusiness, logger) { }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<CompanyConsultDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllCategory");


        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(CompanyConsultDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(CompanyDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CompanyDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "Createitem");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(CompanyDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] CompanyDTO dto) =>
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