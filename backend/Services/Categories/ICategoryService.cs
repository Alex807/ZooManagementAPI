using backend.DTOs.Category;

namespace backend.Services.Categories;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(CategoryQueryDto query);
    Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto request);
    Task<CategoryResponseDto?> UpdateCategoryAsync(int id, UpdateCategoryRequestDto request);
    Task<bool> DeleteCategoryAsync(int id);
    Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name);
    Task<IEnumerable<CategoryResponseDto>> GetCategoriesByAnimalCountAsync(int? min, int? max);
    Task<IEnumerable<CategoryResponseDto>> GetEmptyCategoriesAsync();
} 