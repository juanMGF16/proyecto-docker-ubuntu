using Business.Repository.Interfaces.Specific.System;
using Entity.DTOs.System.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Web.Controllers.Base;

namespace Web.Controllers.System
{
    [Route("api/[controller]/")]
    public class BranchController : BaseController<IBranchBusiness>
    {

        public BranchController(IBranchBusiness branchBusiness, ILogger<BranchController> logger)
            : base(branchBusiness, logger) { }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<BranchConsultDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllCategory");

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(BranchConsultDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        [HttpGet("GetByIdCompany/{id:int}")]
        [ProducesResponseType(typeof(BranchSimpleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdCompany(int id) =>
            await TryExecuteAsync(() => _service.GetBranchesByCompanyAsync(id), "GetByIdCompany");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        [HttpGet("GetBranchDetails/{id:int}")]
        [ProducesResponseType(typeof(BranchDetailsDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBranchDetails(int id) =>
            await TryExecuteAsync(() => _service.GetBranchDetailsAsync(id), "GetBranchDetails");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        [HttpGet("GetInCharge/{id:int}")]
        [ProducesResponseType(typeof(BranchInChargeDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetInCharge(int id) =>
            await TryExecuteAsync(() => _service.GetInChargeAsync(id), "GetInCharge");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR ")]
        [HttpGet("GetInCharges/{companyId:int}")]
        [ProducesResponseType(typeof(IEnumerable<BranchInChargeListDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetZoneInChargesByCompany(int companyId) =>
            await TryExecuteAsync(() => _service.GetInChargesAsync(companyId), "GetInCharges");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR")]
        [HttpGet("GetBranchByInCharge/{id:int}")]
        [ProducesResponseType(typeof(BranchConsultDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBranchByInCharge(int id) =>
            await TryExecuteAsync(() => _service.GetBranchByInChargeAsync(id), "GetBranchByInCharge");

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(BranchDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] BranchDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "Createitem");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(BranchDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] BranchDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "Updateitem");

        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR, SUBADMINISTRADOR")]
        [HttpPatch("PartialUpdate/")]
        [ProducesResponseType(typeof(BranchDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PartialUpdate([FromBody] BranchPartialUpdateDTO dto) =>
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
