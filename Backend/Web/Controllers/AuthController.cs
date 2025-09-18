using Business.Repository.Interfaces.Specific.SecurityModule;
using Business.Services.Jwt;
using Business.Services.Jwt.Interfaces;
using Business.Services.JWTService;
using Business.Services.JWTService.Interfaces;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.DTOs.SecurityModule.Person;
using Entity.DTOs.SecurityModule.User;
using Entity.Models.SecurityModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utilities.Exceptions;

namespace Web.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AppDbContext _context;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IPersonBusiness _personBusiness;

        public AuthController(
            AuthService authService,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            AppDbContext context,
            IRoleBusiness roleBusines,
            IUserBusiness userBusiness,
            IPersonBusiness personBusiness)
        {
            _authService = authService;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _context = context;
            _roleBusiness = roleBusines;
            _userBusiness = userBusiness;
            _personBusiness = personBusiness;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var response = await _authService.AuthenticateAsync(loginRequest);
            if (response == null)
                return Unauthorized("Credenciales inválidas.");

            return Ok(response);
        }

        [HttpPost("LoginOperativo")]
        public async Task<IActionResult> LoginOperativo([FromBody] LoginOperativoDTO loginRequest)
        {
            var response = await _authService.AuthenticateByDocument(loginRequest);
            if (response == null)
                return Unauthorized("Credenciales inválidas.");

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validar Existencia de Numero de Documento y Telefono
                var personDto = new PersonDTO
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DocumentType = dto.DocumentType,
                    DocumentNumber = dto.DocumentNumber,
                    Phone = dto.Phone,
                    Active = true
                };

                var tempUserDto = new UserOptionsDTO
                {
                    Username = dto.Username,
                    Password = dto.Password,
                    PersonId = 0, // temporal, se actualizara despues
                    Active = true
                };

                // Si las validaciones pasan, guardar Person
                var createdPerson = await _personBusiness.CreateAsync(personDto);

                // Crear User con el PersonId correcto
                var userDto = new UserOptionsDTO
                {
                    Username = dto.Username,
                    Password = dto.Password,
                    PersonId = createdPerson.Id,
                    Active = true
                };

                var createdUser = await _userBusiness.CreateAsync(userDto);

                var userRole = new UserRole
                {
                    UserId = createdUser.Id,
                    RoleId = 2,
                    Active = true
                };
                await _context.Set<UserRole>().AddAsync(userRole);

                // Confirmar la transacción
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Registro exitoso." });
            }
            catch (ValidationException ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new
                {
                    error = ex.Message,
                    field = ex.PropertyName
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    error = "Ocurrió un error inesperado.",
                    details = ex.Message
                });
            }
        }


        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleBusiness.GetAllAsync();
            return Ok(roles);
        }

        [HttpPost("Refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            var result = _refreshTokenService.RefreshAccessToken(request.RefreshToken);
            if (result == null)
                return Unauthorized(new { message = "Invalid refresh token" });

            return Ok(result);
        }



    }
}
