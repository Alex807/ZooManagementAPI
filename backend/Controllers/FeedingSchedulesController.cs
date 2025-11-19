using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs.FeedingSchedule;
using backend.Services.FeedingSchedules;
using backend.Enums;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // Visitors can see feeding schedules
public class FeedingSchedulesController : ControllerBase
{
    private readonly IFeedingScheduleService _feedingScheduleService;

    public FeedingSchedulesController(IFeedingScheduleService feedingScheduleService)
    {
        _feedingScheduleService = feedingScheduleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetAllFeedingSchedules([FromQuery] FeedingScheduleQueryDto query)
    {
        var schedules = await _feedingScheduleService.GetAllFeedingSchedulesAsync(query);
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FeedingScheduleResponseDto>> GetFeedingScheduleById(int id)
    {
        var schedule = await _feedingScheduleService.GetFeedingScheduleByIdAsync(id);
        if (schedule == null)
            return NotFound(new { message = $"Feeding schedule with ID {id} not found" });

        return Ok(schedule);
    }

    [HttpPost]
    [Authorize(Policy = "ZookeeperOrAbove")]
    public async Task<ActionResult<FeedingScheduleResponseDto>> CreateFeedingSchedule([FromBody] CreateFeedingScheduleRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var schedule = await _feedingScheduleService.CreateFeedingScheduleAsync(request);
            return CreatedAtAction(nameof(GetFeedingScheduleById), new { id = schedule.Id }, schedule);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ZookeeperOrAbove")]
    public async Task<ActionResult<FeedingScheduleResponseDto>> UpdateFeedingSchedule(int id, [FromBody] UpdateFeedingScheduleRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var schedule = await _feedingScheduleService.UpdateFeedingScheduleAsync(id, request);
            if (schedule == null)
                return NotFound(new { message = $"Feeding schedule with ID {id} not found" });

            return Ok(schedule);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteFeedingSchedule(int id)
    {
        var deleted = await _feedingScheduleService.DeleteFeedingScheduleAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Feeding schedule with ID {id} not found" });

        return NoContent();
    }

    [HttpGet("search/by-animal/{animalId}")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetFeedingSchedulesByAnimal(int animalId)
    {
        var schedules = await _feedingScheduleService.GetFeedingSchedulesByAnimalAsync(animalId);
        return Ok(schedules);
    }

    [HttpGet("search/by-staff/{staffId}")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetFeedingSchedulesByStaff(int staffId)
    {
        var schedules = await _feedingScheduleService.GetFeedingSchedulesByStaffAsync(staffId);
        return Ok(schedules);
    }

    [HttpGet("search/by-status/{status}")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetFeedingSchedulesByStatus(FeedingStatus status)
    {
        var schedules = await _feedingScheduleService.GetFeedingSchedulesByStatusAsync(status);
        return Ok(schedules);
    }

    [HttpGet("search/by-food-type")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetFeedingSchedulesByFoodType([FromQuery] string foodType)
    {
        try
        {
            var schedules = await _feedingScheduleService.GetFeedingSchedulesByFoodTypeAsync(foodType);
            return Ok(schedules);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-date-range")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetFeedingSchedulesByDateRange(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var schedules = await _feedingScheduleService.GetFeedingSchedulesByDateRangeAsync(from, to);
        return Ok(schedules);
    }

    [HttpGet("search/today")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetTodaysFeedingSchedules()
    {
        var schedules = await _feedingScheduleService.GetTodaysFeedingSchedulesAsync();
        return Ok(schedules);
    }

    [HttpGet("search/upcoming")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetUpcomingFeedingSchedules([FromQuery] int hours = 24)
    {
        var schedules = await _feedingScheduleService.GetUpcomingFeedingSchedulesAsync(hours);
        return Ok(schedules);
    }

    [HttpGet("search/overdue")]
    public async Task<ActionResult<IEnumerable<FeedingScheduleResponseDto>>> GetOverdueFeedingSchedules()
    {
        var schedules = await _feedingScheduleService.GetOverdueFeedingSchedulesAsync();
        return Ok(schedules);
    }
}