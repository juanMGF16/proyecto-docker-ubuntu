using Business.Services.JWTService.Interfaces;
using Entity.Context;
using Entity.DTOs.Auth;
using Entity.Models.SecurityModule;
using Microsoft.EntityFrameworkCore;
using Utilities.Helpers;

namespace Business.Services.JWTService
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDTO?> AuthenticateAsync(LoginRequestDTO loginRequest)
        {
            var user = await _context.User
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.Active == true)
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user == null || !PasswordHelper.Verify(user.Password, loginRequest.Password))
                return null;

            var role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "Usuario";

            // Access Token (corto, ej. 10 minutos)
            var accessToken = _jwtService.GenerateToken(user.Id, user.PersonId, user.Username, role, 10);

            // Refresh Token (largo,1440 min = 1 día)
            var refreshToken = _jwtService.GenerateToken(user.Id, user.PersonId, user.Username, role, 1440);

            return new LoginResponseDTO
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponseDTO?> AuthenticateByDocument(LoginOperativoDTO loginRequest)
        {
            var users = await _context.User
                .Include(u => u.Person)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.Active == true &&
                    u.Person.DocumentType == loginRequest.DocumentType &&
                    u.Person.DocumentNumber == loginRequest.DocumentNumber)
                .ToListAsync();

            if (!users.Any())
                return null;

            // Busca el que sea OPERATIVO
            var user = users.FirstOrDefault(u => u.UserRoles.Any(ur => ur.Role.Name == "OPERATIVO"));

            if (user == null)
                return null;

            var role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "OPERATIVO";

            // 🎟️ Generar tokens
            var accessToken = _jwtService.GenerateToken(user.Id, user.PersonId, user.Username, role, 10);
            var refreshToken = _jwtService.GenerateToken(user.Id, user.PersonId, user.Username, role, 1440);

            return new LoginResponseDTO
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }




    }
}
