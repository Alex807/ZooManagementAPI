using backend.DTOs.User;
using backend.DTOs.Authentication;

namespace backend.Services.Authentication;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(UserQueryDto query);
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<UserResponseDto> GetUserByUsernameAsync(string username);
    Task<UserResponseDto> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserResponseDto>> GetUsersByRoleAsync(int roleId);
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto request);
    Task DeleteUserAsync(int id);
    Task AssignRoleToUserAsync(int userId, int roleId);
    Task RemoveRoleFromUserAsync(int userId, int roleId);
    Task<UserResponseDto> ChangeUserCurrentRoleAsync(int userId, int newRoleId);
}