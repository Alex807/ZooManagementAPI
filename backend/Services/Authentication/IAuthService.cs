using backend.DTOs.Authentication;

namespace backend.Services.Authentication;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task ChangePasswordAsync(int userId, ChangePasswordRequestDto request);
    Task<AuthResponseDto> RefreshTokenAsync(int userId);
}