using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.User;
using backend.DTOs.Authentication;
using backend.Services.Authentication;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers([FromQuery] UserQueryDto query)
        {
            var users = await _userService.GetAllUsersAsync(query);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("search/username/{username}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserResponseDto>> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }

        [HttpGet("search/email/{email}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserResponseDto>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpGet("search/role/{roleId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsersByRole(int roleId)
        {
            var users = await _userService.GetUsersByRoleAsync(roleId);
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, [FromBody] UpdateUserRequestDto request)
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != id)
            {
                return Forbid();
            }

            var user = await _userService.UpdateUserAsync(id, request);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("{userId}/roles/{roleId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> AssignRoleToUser(int userId, int roleId)
        {
            await _userService.AssignRoleToUserAsync(userId, roleId);
            return NoContent();
        }

        [HttpDelete("{userId}/roles/{roleId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            await _userService.RemoveRoleFromUserAsync(userId, roleId);
            return NoContent();
        } 

        [HttpPut("{userId}/change-role")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserResponseDto>> ChangeUserRole(int userId, [FromBody] ChangeUserRoleDto request)
        {
            var user = await _userService.ChangeUserCurrentRoleAsync(userId, request.NewRoleId);
            return Ok(user);
        }
    }
}