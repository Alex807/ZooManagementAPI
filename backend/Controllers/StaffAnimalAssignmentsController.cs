using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs.Assignment;
using backend.Services.Assignments;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StaffAnimalAssignmentsController : ControllerBase
{
    private readonly IStaffAnimalAssignmentService _assignmentService;

    public StaffAnimalAssignmentsController(IStaffAnimalAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffAnimalAssignmentResponseDto>>> GetAllAssignments([FromQuery] StaffAnimalAssignmentQueryDto query)
    {
        var assignments = await _assignmentService.GetAllAssignmentsAsync(query);
        return Ok(assignments);
    }

    [HttpGet("{staffId}/{animalId}")]
    public async Task<ActionResult<StaffAnimalAssignmentResponseDto>> GetAssignmentById(int staffId, int animalId)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(staffId, animalId);
        if (assignment == null)
            return NotFound(new { message = $"Assignment for Staff ID {staffId} and Animal ID {animalId} not found" });

        return Ok(assignment);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<StaffAnimalAssignmentResponseDto>> CreateAssignment([FromBody] CreateAssignmentRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var assignment = await _assignmentService.CreateAssignmentAsync(request);
            return CreatedAtAction(nameof(GetAssignmentById), new { staffId = assignment.StaffId, animalId = assignment.AnimalId }, assignment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{staffId}/{animalId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<StaffAnimalAssignmentResponseDto>> UpdateAssignment(
        int staffId,
        int animalId,
        [FromBody] UpdateAssignmentRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var assignment = await _assignmentService.UpdateAssignmentAsync(staffId, animalId, request);
            if (assignment == null)
                return NotFound(new { message = $"Assignment for Staff ID {staffId} and Animal ID {animalId} not found" });

            return Ok(assignment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{staffId}/{animalId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteAssignment(int staffId, int animalId)
    {
        var deleted = await _assignmentService.DeleteAssignmentAsync(staffId, animalId);
        if (!deleted)
            return NotFound(new { message = $"Assignment for Staff ID {staffId} and Animal ID {animalId} not found" });

        return NoContent();
    }

    [HttpGet("search/by-staff/{staffId}")]
    public async Task<ActionResult<IEnumerable<StaffAnimalAssignmentResponseDto>>> GetAssignmentsByStaff(int staffId)
    {
        var assignments = await _assignmentService.GetAssignmentsByStaffAsync(staffId);
        return Ok(assignments);
    }

    [HttpGet("search/by-animal/{animalId}")]
    public async Task<ActionResult<IEnumerable<StaffAnimalAssignmentResponseDto>>> GetAssignmentsByAnimal(int animalId)
    {
        var assignments = await _assignmentService.GetAssignmentsByAnimalAsync(animalId);
        return Ok(assignments);
    }

    [HttpGet("search/with-observations")]
    public async Task<ActionResult<IEnumerable<StaffAnimalAssignmentResponseDto>>> GetAssignmentsWithObservations()
    {
        var assignments = await _assignmentService.GetAssignmentsWithObservationsAsync();
        return Ok(assignments); 
    }

    [HttpGet("search/by-date-range")]
    public async Task<ActionResult<IEnumerable<StaffAnimalAssignmentResponseDto>>> GetAssignmentsByDateRange(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var assignments = await _assignmentService.GetAssignmentsByDateRangeAsync(from, to);
        return Ok(assignments);
    }
}