using backend.DTOs.User;

namespace backend.Services.Authentication;

public interface IUserDetailsService
{
    Task<IEnumerable<UserDetailsDto>> GetAllUserDetailsAsync(UserDetailsQueryDto query);
    Task<UserDetailsDto> GetUserDetailsByIdAsync(int userId);
    Task<IEnumerable<UserDetailsDto>> SearchByNameAsync(string firstName, string? lastName);
    Task<UserDetailsDto> GetUserDetailsByPhoneAsync(string phone);
    Task<UserDetailsDto> UpdateUserDetailsAsync(int userId, UpdateUserDetailsRequestDto request);
    Task DeleteUserDetailsAsync(int userId);
}