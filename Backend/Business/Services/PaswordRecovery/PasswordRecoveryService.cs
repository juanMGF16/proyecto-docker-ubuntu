using System.Security.Cryptography;
using Business.Services.PaswordRecovery.Interfaces;
using Business.Services.SendEmail.Interfaces;
using Data.Repository.Interfaces.Specific.SecurityModule;
using Entity.DTOs.SecurityModule;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;
using Utilities.Templates;

namespace Business.Services.PaswordRecovery
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly IUserData _userData;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PasswordRecoveryService> _logger;

        // Diccionario temporal para almacenar tokens (en producción usar Redis o DB)
        private static readonly Dictionary<string, RecoveryTokenInfo> _recoveryTokens = [];

        public PasswordRecoveryService(
            IUserData userData,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<PasswordRecoveryService> logger)
        {
            _userData = userData;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendPasswordRecoveryEmailAsync(string email)
        {
            try
            {
                ValidationHelper.ThrowIfEmpty(email, "Email");

                // Buscar usuario por email
                var users = await _userData.GetAllAsync();
                var user = users.FirstOrDefault(u =>
                    u.Person != null &&
                    u.Person.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    _logger.LogWarning($"Intento de recuperación para email no registrado: {email}");
                    return true; // Por seguridad, no revelar si el email existe o no
                }

                // Generar token de recuperación
                var token = await GenerateRecoveryTokenAsync(user.Id);
                var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://localhost:7051";
                var recoveryLink = $"{baseUrl}/api/auth/reset-password?token={token}";

                // Crear contenido del email
                var subject = "Recuperación de Contraseña - Tu Sistema";
                var body = EmailTemplates.GetPasswordRecoveryTemplate(user.Username, recoveryLink);

                // Enviar email
                var result = await _emailService.SendEmailAsync(email, subject, body, true);

                if (result)
                {
                    _logger.LogInformation($"Email de recuperación enviado a: {email}");
                    return true;
                }

                _logger.LogError($"Error al enviar email de recuperación a: {email}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en SendPasswordRecoveryEmailAsync para: {email}");
                return false;
            }
        }

        public Task<string> GenerateRecoveryTokenAsync(int userId)
        {
            var tokenBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(tokenBytes);
            var token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            var tokenInfo = new RecoveryTokenInfo
            {
                UserId = userId,
                Expiration = DateTime.UtcNow.AddHours(24),
                Used = false
            };

            _recoveryTokens[token] = tokenInfo;

            return Task.FromResult(token);
        }


        public Task<bool> ValidateRecoveryTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token) || !_recoveryTokens.TryGetValue(token, out var tokenInfo))
                return Task.FromResult(false);

            var isValid = !tokenInfo.Used && tokenInfo.Expiration > DateTime.UtcNow;
            return Task.FromResult(isValid);
        }

        public async Task<bool> ResetPasswordAsync(PasswordResetDTO resetDto)
        {
            try
            {
                ValidationHelper.ThrowIfNull(resetDto, nameof(resetDto));
                ValidationHelper.ThrowIfEmpty(resetDto.Token, "Token");
                ValidationHelper.ThrowIfEmpty(resetDto.NewPassword, "NewPassword");

                // Validar token
                if (!_recoveryTokens.TryGetValue(resetDto.Token, out var tokenInfo) ||
                    tokenInfo.Used || tokenInfo.Expiration <= DateTime.UtcNow)
                {
                    throw new ValidationException("Token", "Token inválido o expirado");
                }

                // Buscar usuario
                var user = await _userData.GetByIdAsync(tokenInfo.UserId);
                if (user == null)
                    throw new EntityNotFoundException(nameof(User), tokenInfo.UserId);

                // Actualizar contraseña
                user.Password = PasswordHelper.Hash(resetDto.NewPassword);
                await _userData.UpdateAsync(user);

                // Marcar token como usado
                tokenInfo.Used = true;
                _recoveryTokens[resetDto.Token] = tokenInfo;

                _logger.LogInformation($"Contraseña restablecida para usuario: {user.Id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en ResetPasswordAsync");
                throw;
            }
        }

        // Clase interna para almacenar información del token
        private class RecoveryTokenInfo
        {
            public int UserId { get; set; }
            public DateTime Expiration { get; set; }
            public bool Used { get; set; }
        }
    }
}
