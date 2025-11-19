using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs.MedicalRecord;
using backend.Services.MedicalRecords;
using backend.Enums;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAbove")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordsController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponseDto>>> GetAllMedicalRecords([FromQuery] MedicalRecordQueryDto query)
    {
        var records = await _medicalRecordService.GetAllMedicalRecordsAsync(query);
        return Ok(records);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalRecordResponseDto>> GetMedicalRecordById(int id)
    {
        var record = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
        if (record == null)
            return NotFound(new { message = $"Medical record with ID {id} not found" });

        return Ok(record);
    }

    [HttpPost]
    [Authorize(Policy = "VeterinarianOrAbove")]
    public async Task<ActionResult<MedicalRecordResponseDto>> CreateMedicalRecord([FromBody] CreateMedicalRecordRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var record = await _medicalRecordService.CreateMedicalRecordAsync(request);
            return CreatedAtAction(nameof(GetMedicalRecordById), new { id = record.Id }, record);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "VeterinarianOrAbove")]
    public async Task<ActionResult<MedicalRecordResponseDto>> UpdateMedicalRecord(int id, [FromBody] UpdateMedicalRecordRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var record = await _medicalRecordService.UpdateMedicalRecordAsync(id, request);
        if (record == null)
            return NotFound(new { message = $"Medical record with ID {id} not found" });

        return Ok(record);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteMedicalRecord(int id)
    {
        var deleted = await _medicalRecordService.DeleteMedicalRecordAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Medical record with ID {id} not found" });

        return NoContent();
    }

    [HttpGet("search/by-animal/{animalId}")]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponseDto>>> GetMedicalRecordsByAnimal(int animalId)
    {
        var records = await _medicalRecordService.GetMedicalRecordsByAnimalAsync(animalId);
        return Ok(records);
    }

    [HttpGet("search/by-staff/{staffId}")]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponseDto>>> GetMedicalRecordsByStaff(int staffId)
    {
        var records = await _medicalRecordService.GetMedicalRecordsByStaffAsync(staffId);
        return Ok(records);
    }

    [HttpGet("search/by-status/{status}")]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponseDto>>> GetMedicalRecordsByStatus(HealthStatus status)
    {
        var records = await _medicalRecordService.GetMedicalRecordsByStatusAsync(status);
        return Ok(records);
    }

    [HttpGet("search/by-date-range")]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponseDto>>> GetMedicalRecordsByDateRange(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var records = await _medicalRecordService.GetMedicalRecordsByDateRangeAsync(from, to);
        return Ok(records);
    }

    [HttpGet("search/recent")]
    public async Task<ActionResult<IEnumerable<MedicalRecordResponseDto>>> GetRecentMedicalRecords([FromQuery] int days = 7)
    {
        var records = await _medicalRecordService.GetRecentMedicalRecordsAsync(days);
        return Ok(records);
    }
}