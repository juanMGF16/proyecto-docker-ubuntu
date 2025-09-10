using Business.Services.Jwt.Interfaces;
using Business.Services.JWTService.Interfaces;
using Entity.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Jwt
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(IJwtService jwtService, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public RefreshResponseDTO? RefreshAccessToken(string refreshToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

                var claimsPrincipal = handler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                var identity = claimsPrincipal.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated)
                    return null;

                var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var personId = int.Parse(identity.FindFirst("personId")!.Value);
                var username = identity.FindFirst(ClaimTypes.Name)!.Value;
                var role = identity.FindFirst(ClaimTypes.Role)!.Value;

                // Generar un nuevo access token corto
                var newAccessToken = _jwtService.GenerateToken(userId, personId, username, role, 5);

                return new RefreshResponseDTO { Token = newAccessToken };
            }
            catch
            {
                return null;
            }
        }
    }
}
