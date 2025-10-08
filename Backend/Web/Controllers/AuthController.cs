using Business.Repository.Interfaces.Specific.ParametersModule;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Business.Services.Jwt;
using Business.Services.Jwt.Interfaces;
using Business.Services.PaswordRecovery.Interfaces;
using Business.Services.SendEmail.Interfaces;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.DTOs.SecurityModule.Person;
using Entity.DTOs.SecurityModule.User;
using Entity.Models.SecurityModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using Utilities.Templates;

namespace Web.Controllers
{
    /// <summary>
    /// Controller para autenticación, registro y recuperación de contraseñas
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AppDbContext _context;
        private readonly INotificationBusiness _notificationBusiness;
        private readonly IEmailService _emailService;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IPersonBusiness _personBusiness;
        private readonly IPasswordRecoveryService _passwordRecoveryService;

        public AuthController(
            AuthService authService,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            AppDbContext context,
            INotificationBusiness notificationBusiness,
            IEmailService emailService,
            IRoleBusiness roleBusines,
            IUserBusiness userBusiness,
            IPersonBusiness personBusiness,
            IPasswordRecoveryService passwordRecoveryService)
        {
            _authService = authService;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _context = context;
            _notificationBusiness = notificationBusiness;
            _emailService = emailService;
            _roleBusiness = roleBusines;
            _userBusiness = userBusiness;
            _personBusiness = personBusiness;
            _passwordRecoveryService = passwordRecoveryService;
        }

        /// <summary>
        /// Autentica un usuario con credenciales estándar (username/password)
        /// </summary>
        /// <param name="loginRequest">Credenciales de inicio de sesión</param>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var response = await _authService.AuthenticateAsync(loginRequest);
            if (response == null)
                return Unauthorized("Credenciales inválidas.");

            return Ok(response);
        }

        /// <summary>
        /// Autentica un usuario operativo usando documento de identidad
        /// </summary>
        /// <param name="loginRequest">Credenciales con documento</param>
        [HttpPost("LoginOperativo")]
        public async Task<IActionResult> LoginOperativo([FromBody] LoginOperativoDTO loginRequest)
        {
            var response = await _authService.AuthenticateByDocument(loginRequest);
            if (response == null)
                return Unauthorized("Credenciales inválidas.");

            return Ok(response);
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema con envío de email de bienvenida
        /// </summary>
        /// <param name="dto">Datos del usuario a registrar</param>
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

                // ================== [ EMAIL DE BIENVENIDA ] ==================
                var loginLink = $"http://localhost:4200/Login";
                var welcomeBody = EmailTemplates.GetWelcomeTemplate(createdUser.Username, loginLink);

                await _emailService.SendEmailAsync(
                    createdPerson.Email,
                    "🎉 Bienvenido a Codexy",
                    welcomeBody,
                    true
                );

                return Ok(new { message = "Registro exitoso. Revisa tu correo electrónico 📩" });
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

        /// <summary>
        /// Obtiene lista de todos los roles disponibles
        /// </summary>
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleBusiness.GetAllAsync();
            return Ok(roles);
        }


        // ================== [ RECUPERAR CONTRASEÑA ] ==================

        /// <summary>
        /// Envía email con enlace de recuperación de contraseña
        /// </summary>
        /// <param name="request">Email del usuario</param>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordRecoveryRequestDTO request)
        {
            try
            {
                var result = await _passwordRecoveryService.SendPasswordRecoveryEmailAsync(request.Email);

                if (result)
                {
                    // Log de notificación exitosa
                    await _notificationBusiness.LogNotificationAsync(
                        1, // SecurityModule
                        "Solicitud de Recuperación de Contraseña",
                        $"Se envió un email de recuperación a: {request.Email}",
                        "PasswordResetEmail"
                    );

                    return Ok(new
                    {
                        success = true,
                        message = "Si el email está registrado, recibirás instrucciones para recuperar tu contraseña"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Error al procesar la solicitud de recuperación"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Valida si un token de recuperación es válido y no ha expirado
        /// </summary>
        /// <param name="token">Token a validar</param>
        [HttpGet("validate-recovery-token")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateRecoveryToken([FromQuery] string token)
        {
            try
            {
                var (isValid, email) = await _passwordRecoveryService.ValidateRecoveryTokenWithEmailAsync(token);

                return Ok(new
                {
                    success = true,
                    valid = isValid,
                    email = isValid ? email : null,
                    message = isValid ? "Token válido" : "Token inválido o expirado"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    valid = false,
                    message = "Error validando el token",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Restablece la contraseña usando un token válido
        /// </summary>
        /// <param name="request">Token y nueva contraseña</param>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDTO request)
        {
            try
            {
                var result = await _passwordRecoveryService.ResetPasswordAsync(request);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Contraseña restablecida exitosamente"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Error al restablecer la contraseña"
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    field = ex.PropertyName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Reenvía email de recuperación de contraseña
        /// </summary>
        /// <param name="request">Email del usuario</param>
        [HttpPost("resend-recovery-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendRecoveryEmail([FromBody] PasswordRecoveryRequestDTO request)
        {
            try
            {
                var result = await _passwordRecoveryService.SendPasswordRecoveryEmailAsync(request.Email);

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Email de recuperación reenviado"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Error al reenviar el email de recuperación"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }
    }
}
