﻿using System.Security.Claims;
using Business.Repository.Implementations.Specific.SecurityModule;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Entity.DTOs.SecurityModule.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Utilities.Exceptions;
using Web.Controllers.Base;

namespace Web.Controllers.SecurityModel
{
    [Route("api/[controller]")]
    public class UserController : BaseController<IUserBusiness>
    {
        public UserController(IUserBusiness userBusiness, ILogger<UserController> logger)
            : base(userBusiness, logger) { }

        [HttpGet("GetAll/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllUsers");

        [HttpGet("GetAllJWT/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<IActionResult> GetAllJWT()
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var role = roleClaim?.Value;

            if (string.Equals(role, "SM_ACTION", StringComparison.OrdinalIgnoreCase))
            {
                return await TryExecuteAsync(async () =>
                {
                    if (_service is UserBusiness ubGeneral)
                    {
                        return await ubGeneral.GetAllTotalUsersAsync();
                    }
                    throw new ValidationException("Funcionalidad no disponible para este tipo de negocio.");
                }, "GetAllTotalUsers");
            }

            return await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllUsers");
        }

        [HttpGet("GetByUsername/{username}")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByUsername(string username) =>
         await TryExecuteAsync(() => _service.GetByUsernameAsync(username), "GetByUsername");

        [HttpGet("GetById/{id:int}")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpGet("HasCompany/")]
        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HasCompany()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "No se pudo obtener el ID del usuario." });

            if (!int.TryParse(userIdClaim, out var userId))
                return BadRequest(new { message = "El ID del usuario no es válido." });

            return await TryExecuteAsync(() => _service.HasCompanyAsync(userId), "HasCompnay");
        }

        [HttpPost("Create/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(UserOptionsDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UserOptionsDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "CreateUser");
        }

        [HttpPut("Update/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(UserOptionsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] UserOptionsDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "UpdateUser");

        [HttpPatch("PartialUpdate/")]
        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PartialUpdate([FromBody] UserPartialUpdateDTO dto) =>
            await TryExecuteAsync(() => _service.PartialUpdateAsync(dto), "PartialUpdateUser");

        [HttpPost("ChangePassword/")]
        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "No se pudo obtener el ID del usuario." });

            if (!int.TryParse(userIdClaim, out var userId))
                return BadRequest(new { message = "El ID del usuario no es válido." });

            return await TryExecuteAsync(async () =>
            {
                await _service.ChangePasswordAsync(userId, dto);
                return Ok(new { message = "Contraseña cambiada correctamente" });
            }, "ChangePassword");
        }

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