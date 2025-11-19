using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs.Animal;
using backend.Services.Animals;
using backend.Enums;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalsController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAllAnimals([FromQuery] AnimalQueryDto query)
    {
        var animals = await _animalService.GetAllAnimalsAsync(query);
        return Ok(animals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalResponseDto>> GetAnimalById(int id)
    {
        var animal = await _animalService.GetAnimalByIdAsync(id);
        if (animal == null)
            return NotFound(new { message = $"Animal with ID {id} not found" });

        return Ok(animal);
    }

    [HttpPost]
    [Authorize(Policy = "ZookeeperOrAbove")]
    public async Task<ActionResult<AnimalResponseDto>> CreateAnimal([FromBody] CreateAnimalRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var animal = await _animalService.CreateAnimalAsync(request);
            return CreatedAtAction(nameof(GetAnimalById), new { id = animal.Id }, animal);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ZookeeperOrAbove")]
    public async Task<ActionResult<AnimalResponseDto>> UpdateAnimal(int id, [FromBody] UpdateAnimalRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var animal = await _animalService.UpdateAnimalAsync(id, request);
            if (animal == null)
                return NotFound(new { message = $"Animal with ID {id} not found" });

            return Ok(animal);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        var deleted = await _animalService.DeleteAnimalAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Animal with ID {id} not found" });

        return NoContent();
    }

    [HttpGet("search/by-category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAnimalsByCategory(int categoryId)
    {
        var animals = await _animalService.GetAnimalsByCategoryAsync(categoryId);
        return Ok(animals);
    }

    [HttpGet("search/by-enclosure/{enclosureId}")]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAnimalsByEnclosure(int enclosureId)
    {
        var animals = await _animalService.GetAnimalsByEnclosureAsync(enclosureId);
        return Ok(animals);
    }

    [HttpGet("search/by-specie")]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAnimalsBySpecie([FromQuery] string specie)
    {
        try
        {
            var animals = await _animalService.GetAnimalsBySpecieAsync(specie);
            return Ok(animals);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-gender/{gender}")]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAnimalsByGender(Gender gender)
    {
        var animals = await _animalService.GetAnimalsByGenderAsync(gender);
        return Ok(animals);
    }

    [HttpGet("search/by-age-range")]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAnimalsByAgeRange(
        [FromQuery] int? minAge,
        [FromQuery] int? maxAge)
    {
        var animals = await _animalService.GetAnimalsByAgeRangeAsync(minAge, maxAge);
        return Ok(animals);
    }

    [HttpGet("search/by-arrival-date")]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> GetAnimalsByArrivalDate(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var animals = await _animalService.GetAnimalsByArrivalDateAsync(from, to);
        return Ok(animals);
    }
}