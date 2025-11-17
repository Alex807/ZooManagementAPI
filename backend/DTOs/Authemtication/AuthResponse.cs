using backend.DTOs.User;

namespace backend.DTOs.Authemtication;

// DTO for authentication response containing JWT token and user information
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserResponseDto User { get; set; } = null!;
}