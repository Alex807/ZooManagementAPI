namespace backend.DTOs.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CurrentRole { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public UserDetailsDto? Details { get; set; }
}