namespace Business.Services.JWTService.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, int personId, string username, string role, int expiresInMinutes);
    }
}