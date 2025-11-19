using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs.Staff;
using backend.Services.Staff;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffResponseDto>>> GetAllStaff([FromQuery] StaffQueryDto query)
    {
        var staff = await _staffService.GetAllStaffAsync(query);
        return Ok(staff);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StaffResponseDto>> GetStaffById(int id)
    {
        var staff = await _staffService.GetStaffByIdAsync(id);
        if (staff == null)
            return NotFound(new { message = $"Staff with ID {id} not found" });

        return Ok(staff);
    }

    [HttpPost]
    public async Task<ActionResult<StaffResponseDto>> CreateStaff([FromBody] CreateStaffRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var staff = await _staffService.CreateStaffAsync(request);
            return CreatedAtAction(nameof(GetStaffById), new { id = staff.Id }, staff);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StaffResponseDto>> UpdateStaff(int id, [FromBody] UpdateStaffRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staff = await _staffService.UpdateStaffAsync(id, request);
        if (staff == null)
            return NotFound(new { message = $"Staff with ID {id} not found" });

        return Ok(staff);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        try
        {
            var deleted = await _staffService.DeleteStaffAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Staff with ID {id} not found" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-department")]
    public async Task<ActionResult<IEnumerable<StaffResponseDto>>> GetStaffByDepartment([FromQuery] string department)
    {
        try
        {
            var staff = await _staffService.GetStaffByDepartmentAsync(department);
            return Ok(staff);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-position")]
    public async Task<ActionResult<IEnumerable<StaffResponseDto>>> GetStaffByPosition([FromQuery] string position)
    {
        try
        {
            var staff = await _staffService.GetStaffByPositionAsync(position);
            return Ok(staff);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-salary-range")]
    public async Task<ActionResult<IEnumerable<StaffResponseDto>>> GetStaffBySalaryRange(
        [FromQuery] decimal? min,
        [FromQuery] decimal? max)
    {
        var staff = await _staffService.GetStaffBySalaryRangeAsync(min, max);
        return Ok(staff);
    }

    [HttpGet("search/by-hire-date")]
    public async Task<ActionResult<IEnumerable<StaffResponseDto>>> GetStaffByHireDate(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var staff = await _staffService.GetStaffByHireDateAsync(from, to);
        return Ok(staff);
    }

    [HttpGet("search/by-user-account/{userAccountId}")]
    public async Task<ActionResult<StaffResponseDto>> GetStaffByUserAccountId(int userAccountId)
    {
        var staff = await _staffService.GetStaffByUserAccountIdAsync(userAccountId);
        if (staff == null)
            return NotFound(new { message = $"Staff with user account ID {userAccountId} not found" });

        return Ok(staff);
    }
}