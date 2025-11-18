using Microsoft.AspNetCore.Mvc;
using backend.DTOs.Category;
using backend.Services.Categories;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories([FromQuery] CategoryQueryDto query)
    {
        var categories = await _categoryService.GetAllCategoriesAsync(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound(new { message = $"Category with ID {id} not found" });

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var category = await _categoryService.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] UpdateCategoryRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var category = await _categoryService.UpdateCategoryAsync(id, request);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found" });

            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Category with ID {id} not found" });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-name")]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategoriesByName([FromQuery] string name)
    {
        try
        {
            var categories = await _categoryService.GetCategoriesByNameAsync(name);
            return Ok(categories);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("search/by-animal-count")]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategoriesByAnimalCount(
        [FromQuery] int? min,
        [FromQuery] int? max)
    {
        var categories = await _categoryService.GetCategoriesByAnimalCountAsync(min, max);
        return Ok(categories);
    }

    [HttpGet("search/empty")]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetEmptyCategories()
    {
        var categories = await _categoryService.GetEmptyCategoriesAsync();
        return Ok(categories);
    }
}