using User.Domain.Dtos.Auth;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Domain.Utilities;

namespace User.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<DefaultServiceResponseDto> RegisterAsync(RegisterDto registerDto, bool isAdmin);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<DefaultServiceResponseDto> RevokeAsync(string userName);
    Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken, string userName);
}
