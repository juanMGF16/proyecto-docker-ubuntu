﻿using System.Security.Claims;
using Business.Repository.Implementations.Specific.SecurityModule;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Entity.DTOs.SecurityModule.FormModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Utilities.Exceptions;
using Web.Controllers.Base;

namespace Web.Controllers.SecurityModel
{
    [Route("api/[controller]")]
    [Authorize(Roles = "SM_ACTION")]

    public class FormModuleController : BaseController<IFormModuleBusiness>
    {
        public FormModuleController(IFormModuleBusiness formModuleBusiness, ILogger<FormModuleController> logger)
            : base(formModuleBusiness, logger) { }

        [HttpGet("GetAll/")]
        [ProducesResponseType(typeof(IEnumerable<FormModuleDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllFormModules");

        [HttpGet("GetAllJWT/")]
        [ProducesResponseType(typeof(IEnumerable<FormModuleDTO>), 200)]
        public async Task<IActionResult> GetAllJWT()
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var role = roleClaim?.Value;

            if (string.Equals(role, "SM_ACTION", StringComparison.OrdinalIgnoreCase))
            {
                return await TryExecuteAsync(async () =>
                {
                    if (_service is FormModuleBusiness fmbGeneral)
                    {
                        return await fmbGeneral.GetAllTotalFormModulesAsync();
                    }
                    throw new ValidationException("Funcionalidad no disponible para este tipo de negocio.");
                }, "GetAllTotalFormModules");
            }

            return await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllFormModules");
        }

        [HttpGet("GetById/{id:int}")]
        [ProducesResponseType(typeof(FormModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpPost("Create/")]
        [ProducesResponseType(typeof(FormModuleOptionsDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] FormModuleOptionsDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "CreateFormModule");
        }

        [HttpPut("Update/")]
        [ProducesResponseType(typeof(FormModuleOptionsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] FormModuleOptionsDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "UpdateFormModule");

        [HttpDelete("Delete/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id, [FromQuery] DeleteType strategy = DeleteType.Logical)
        {
            return await TryExecuteAsync(() => _service.DeleteAsync(id, strategy), "DeleteRole");
        }
    }
}