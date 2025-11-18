using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs.User;
using backend.Services.Authentication;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/user-details")]
    [Authorize] 
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsService _userDetailsService;

        public UserDetailsController(IUserDetailsService userDetailsService)
        {
            _userDetailsService = userDetailsService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetAllUserDetails([FromQuery] UserDetailsQueryDto query)
        {
            var userDetails = await _userDetailsService.GetAllUserDetailsAsync(query);
            return Ok(userDetails);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetailsById(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != userId)
            {
                return Forbid();
            }

            var userDetails = await _userDetailsService.GetUserDetailsByIdAsync(userId);
            return Ok(userDetails);
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserDetailsDto>> GetCurrentUserDetails()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var userDetails = await _userDetailsService.GetUserDetailsByIdAsync(userId);
            return Ok(userDetails);
        }

        [HttpGet("search/name")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> SearchByName([FromQuery] string firstName, [FromQuery] string? lastName)
        {
            var userDetails = await _userDetailsService.SearchByNameAsync(firstName, lastName);
            return Ok(userDetails);
        }

        [HttpGet("search/phone/{phone}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetailsByPhone(string phone)
        {
            var userDetails = await _userDetailsService.GetUserDetailsByPhoneAsync(phone);
            return Ok(userDetails);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UserDetailsDto>> UpdateUserDetails(int userId, [FromBody] UpdateUserDetailsRequestDto request)
        {
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != userId)
            {
                return Forbid();
            }

            var userDetails = await _userDetailsService.UpdateUserDetailsAsync(userId, request);
            return Ok(userDetails);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteUserDetails(int userId)
        {
            await _userDetailsService.DeleteUserDetailsAsync(userId);
            return NoContent();
        }
    }
}