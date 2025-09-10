using Entity.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Jwt.Interfaces
{
    public interface IRefreshTokenService
    {
        RefreshResponseDTO? RefreshAccessToken(string refreshToken);
    }
}
