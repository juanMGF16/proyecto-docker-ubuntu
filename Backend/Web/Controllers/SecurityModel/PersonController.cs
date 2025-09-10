using System.Security.Claims;
using Business.Repository.Implementations.Specific.SecurityModule;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Entity.DTOs.SecurityModule.Person;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Enums;
using Utilities.Exceptions;
using Web.Controllers.Base;

namespace Web.Controllers.SecurityModel
{
    [Route("api/[controller]/")]
    public class PersonController : BaseController<IPersonBusiness>
    {

        public PersonController(IPersonBusiness personBusiness, ILogger<PersonController> logger)
            : base(personBusiness, logger) { }

        [HttpGet("GetAll/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        public async Task<IActionResult> GetAll() =>
            await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllPersons");

        [HttpGet("GetAllJWT/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        public async Task<IActionResult> GetAllJWT()
        {
            var personClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var person = personClaim?.Value;

            if (string.Equals(person, "SM_ACTION", StringComparison.OrdinalIgnoreCase))
            {
                return await TryExecuteAsync(async () =>
                {
                    if (_service is PersonBusiness fbGeneral)
                    {
                        return await fbGeneral.GetAllTotalPersonsAsync();
                    }
                    throw new ValidationException("Funcionalidad no disponible para este tipo de negocio.");
                }, "GetAllTotalPersons");
            }

            return await TryExecuteAsync(() => _service.GetAllAsync(), "GetAllUsers");
        }

        [HttpGet("GetAvailable/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        public async Task<IActionResult> GetAvailable() =>
            await TryExecuteAsync(() => _service.GetPersonAvailableAsync(), "GetAvailablePersons");

        [HttpGet("GetById/{id:int}")]
        [Authorize(Roles = "SM_ACTION,ADMINISTRADOR")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id) =>
            await TryExecuteAsync(() => _service.GetByIdAsync(id), "GetById");

        [HttpPost("Create/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(PersonDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] PersonDTO dto)
        {
            return await TryExecuteAsync(async () =>
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }, "CreatePerson");
        }

        [HttpPut("Update/")]
        [Authorize(Roles = "SM_ACTION")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromBody] PersonDTO dto) =>
            await TryExecuteAsync(() => _service.UpdateAsync(dto), "UpdatePerson");

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