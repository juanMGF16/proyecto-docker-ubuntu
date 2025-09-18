using Entity.DTOs.SecurityModule;

namespace Business.Services.PaswordRecovery.Interfaces
{
    public interface IPasswordRecoveryService
    {
        Task<bool> SendPasswordRecoveryEmailAsync(string email);
        Task<bool> ValidateRecoveryTokenAsync(string token);
        Task<bool> ResetPasswordAsync(PasswordResetDTO resetDto);
        Task<string> GenerateRecoveryTokenAsync(int userId);
    }
}
