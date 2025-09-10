using Entity.DTOs.Auth;

namespace Business.Services.PaswordRecovery.Interfaces
{
    public interface IPasswordRecoveryService
    {
        Task<bool> SendPasswordRecoveryEmailAsync(string email);
        Task<(bool isValid, string? email)> ValidateRecoveryTokenWithEmailAsync(string token);
        Task<bool> ResetPasswordAsync(PasswordResetDTO resetDto);
        Task<string> GenerateRecoveryTokenAsync(int userId, string email);
    }
}
