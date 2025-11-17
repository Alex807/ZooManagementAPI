using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.Models;
using backend.DTOs.Category;

namespace backend.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly ZooManagementDbContext _context;

    public CategoryService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(CategoryQueryDto query)
    {
        var dbQuery = _context.Categories
            .Include(c => c.Animals)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            dbQuery = dbQuery.Where(c => c.Name.Contains(query.Name));

        var categories = await dbQuery.ToListAsync();

        if (query.MinAnimalCount.HasValue)
            categories = categories.Where(c => c.Animals.Count >= query.MinAnimalCount.Value).ToList();

        if (query.MaxAnimalCount.HasValue)
            categories = categories.Where(c => c.Animals.Count <= query.MaxAnimalCount.Value).ToList();

        return categories.Adapt<List<CategoryResponseDto>>();
    }

    public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Animals)
            .FirstOrDefaultAsync(c => c.Id == id);

        return category?.Adapt<CategoryResponseDto>();
    }

    public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto request)
    {
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == request.Name);

        if (existingCategory != null)
            throw new InvalidOperationException($"Category with name '{request.Name}' already exists");

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var createdCategory = await _context.Categories
            .Include(c => c.Animals)
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        return createdCategory!.Adapt<CategoryResponseDto>();
    }

    public async Task<CategoryResponseDto?> UpdateCategoryAsync(int id, UpdateCategoryRequestDto request)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.Id != id);

            if (existingCategory != null)
                throw new InvalidOperationException($"Category with name '{request.Name}' already exists");

            category.Name = request.Name;
        }

        if (request.Description != null)
            category.Description = request.Description;

        if (!string.IsNullOrWhiteSpace(request.ImageUrl))
            category.ImageUrl = request.ImageUrl;

        await _context.SaveChangesAsync();

        var updatedCategory = await _context.Categories
            .Include(c => c.Animals)
            .FirstOrDefaultAsync(c => c.Id == id);

        return updatedCategory!.Adapt<CategoryResponseDto>();
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Animals)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return false;

        if (category.Animals.Any())
            throw new InvalidOperationException($"Cannot delete category '{category.Name}' because it has {category.Animals.Count} associated animals");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name parameter is required");

        var categories = await _context.Categories
            .Include(c => c.Animals)
            .Where(c => c.Name.Contains(name))
            .ToListAsync();

        return categories.Adapt<List<CategoryResponseDto>>();
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByAnimalCountAsync(int? min, int? max)
    {
        var categories = await _context.Categories
            .Include(c => c.Animals)
            .ToListAsync();

        if (min.HasValue)
            categories = categories.Where(c => c.Animals.Count >= min.Value).ToList();

        if (max.HasValue)
            categories = categories.Where(c => c.Animals.Count <= max.Value).ToList();

        return categories.Adapt<List<CategoryResponseDto>>();
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetEmptyCategoriesAsync()
    {
        var categories = await _context.Categories
            .Include(c => c.Animals)
            .Where(c => !c.Animals.Any())
            .ToListAsync();

        return categories.Adapt<List<CategoryResponseDto>>();
    }
}