using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs.Enclosure;
using backend.Services.Enclosures;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnclosuresController : ControllerBase
{
    private readonly IEnclosureService _enclosureService;

    public EnclosuresController(IEnclosureService enclosureService)
    {
        _enclosureService = enclosureService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetAllEnclosures([FromQuery] EnclosureQueryDto query)
    {
        var enclosures = await _enclosureService.GetAllEnclosuresAsync(query);
        return Ok(enclosures);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnclosureResponseDto>> GetEnclosureById(int id)
    {
        var enclosure = await _enclosureService.GetEnclosureByIdAsync(id);
        if (enclosure == null)
            return NotFound(new { message = $"Enclosure with ID {id} not found" });

        return Ok(enclosure);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<EnclosureResponseDto>> CreateEnclosure([FromBody] CreateEnclosureRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var enclosure = await _enclosureService.CreateEnclosureAsync(request);
            return CreatedAtAction(nameof(GetEnclosureById), new { id = enclosure.Id }, enclosure);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<EnclosureResponseDto>> UpdateEnclosure(int id, [FromBody] UpdateEnclosureRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var enclosure = await _enclosureService.UpdateEnclosureAsync(id, request);
            if (enclosure == null)
                return NotFound(new { message = $"Enclosure with ID {id} not found" });

            return Ok(enclosure);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteEnclosure(int id)
    {
        try
        {
            var deleted = await _enclosureService.DeleteEnclosureAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Enclosure with ID {id} not found" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-name")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetEnclosuresByName([FromQuery] string name)
    {
        try
        {
            var enclosures = await _enclosureService.GetEnclosuresByNameAsync(name);
            return Ok(enclosures);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-type")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetEnclosuresByType([FromQuery] string type)
    {
        try
        {
            var enclosures = await _enclosureService.GetEnclosuresByTypeAsync(type);
            return Ok(enclosures);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-location")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetEnclosuresByLocation([FromQuery] string location)
    {
        try
        {
            var enclosures = await _enclosureService.GetEnclosuresByLocationAsync(location);
            return Ok(enclosures);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-capacity")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetEnclosuresByCapacity(
        [FromQuery] int? min,
        [FromQuery] int? max)
    {
        var enclosures = await _enclosureService.GetEnclosuresByCapacityAsync(min, max);
        return Ok(enclosures);
    }

    [HttpGet("search/at-capacity")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetEnclosuresAtCapacity()
    {
        var enclosures = await _enclosureService.GetEnclosuresAtCapacityAsync();
        return Ok(enclosures);
    }

    [HttpGet("search/available")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetAvailableEnclosures()
    {
        var enclosures = await _enclosureService.GetAvailableEnclosuresAsync();
        return Ok(enclosures);
    }

    [HttpGet("search/empty")]
    public async Task<ActionResult<IEnumerable<EnclosureResponseDto>>> GetEmptyEnclosures()
    {
        var enclosures = await _enclosureService.GetEmptyEnclosuresAsync();
        return Ok(enclosures);
    }
}