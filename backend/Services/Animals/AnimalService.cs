using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.Models;
using backend.DTOs.Animal;
using backend.Enums;

namespace backend.Services.Animals;

public class AnimalService : IAnimalService
{
    private readonly ZooManagementDbContext _context;

    public AnimalService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAllAnimalsAsync(AnimalQueryDto query)
    {
        var dbQuery = _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            dbQuery = dbQuery.Where(a => a.Name.Contains(query.Name));

        if (!string.IsNullOrWhiteSpace(query.Specie))
            dbQuery = dbQuery.Where(a => a.Specie.Contains(query.Specie));

        if (query.Gender.HasValue)
            dbQuery = dbQuery.Where(a => a.Gender == query.Gender.Value);

        if (query.CategoryId.HasValue)
            dbQuery = dbQuery.Where(a => a.CategoryId == query.CategoryId.Value);

        if (query.EnclosureId.HasValue)
            dbQuery = dbQuery.Where(a => a.EnclosureId == query.EnclosureId.Value);

        if (query.ArrivalDateFrom.HasValue)
            dbQuery = dbQuery.Where(a => a.ArrivalDate >= query.ArrivalDateFrom.Value);

        if (query.ArrivalDateTo.HasValue)
            dbQuery = dbQuery.Where(a => a.ArrivalDate <= query.ArrivalDateTo.Value);

        var animals = await dbQuery.ToListAsync();

        if (query.MinAge.HasValue || query.MaxAge.HasValue)
        {
            animals = animals.Where(a =>
            {
                if (!a.DateOfBirth.HasValue) return false;
                var age = DateTime.Now.Year - a.DateOfBirth.Value.Year;
                if (query.MinAge.HasValue && age < query.MinAge.Value) return false;
                if (query.MaxAge.HasValue && age > query.MaxAge.Value) return false;
                return true;
            }).ToList();
        }

        return animals.Adapt<List<AnimalResponseDto>>();
    }

    public async Task<AnimalResponseDto?> GetAnimalByIdAsync(int id)
    {
        var animal = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .FirstOrDefaultAsync(a => a.Id == id);

        return animal?.Adapt<AnimalResponseDto>();
    }

    public async Task<AnimalResponseDto> CreateAnimalAsync(CreateAnimalRequestDto request)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (!categoryExists)
            throw new InvalidOperationException($"Category with ID {request.CategoryId} does not exist");

        if (request.EnclosureId.HasValue)
        {
            var enclosureExists = await _context.Enclosures.AnyAsync(e => e.Id == request.EnclosureId.Value);
            if (!enclosureExists)
                throw new InvalidOperationException($"Enclosure with ID {request.EnclosureId.Value} does not exist");

            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == request.EnclosureId.Value);

            if (enclosure != null && enclosure.Animals.Count >= enclosure.Capacity)
                throw new InvalidOperationException($"Enclosure {enclosure.Name} is at full capacity");
        }

        var animal = new Animal
        {
            Name = request.Name,
            Specie = request.Specie,
            ImageUrl = request.ImageUrl,
            DateOfBirth = request.DateOfBirth.HasValue ? DateOnly.FromDateTime(request.DateOfBirth.Value) : null,
            Gender = request.Gender,
            ArrivalDate = request.ArrivalDate ?? DateTime.UtcNow,
            CategoryId = request.CategoryId,
            EnclosureId = request.EnclosureId
        };

        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        var createdAnimal = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .FirstOrDefaultAsync(a => a.Id == animal.Id);

        return createdAnimal!.Adapt<AnimalResponseDto>();
    }

    public async Task<AnimalResponseDto?> UpdateAnimalAsync(int id, UpdateAnimalRequestDto request)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal == null)
            return null;

        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId.Value);
            if (!categoryExists)
                throw new InvalidOperationException($"Category with ID {request.CategoryId.Value} does not exist");
            animal.CategoryId = request.CategoryId.Value;
        }

        if (request.EnclosureId.HasValue)
        {
            var enclosureExists = await _context.Enclosures.AnyAsync(e => e.Id == request.EnclosureId.Value);
            if (!enclosureExists)
                throw new InvalidOperationException($"Enclosure with ID {request.EnclosureId.Value} does not exist");

            var enclosure = await _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == request.EnclosureId.Value);

            if (enclosure != null && animal.EnclosureId != request.EnclosureId.Value && 
                enclosure.Animals.Count >= enclosure.Capacity)
                throw new InvalidOperationException($"Enclosure {enclosure.Name} is at full capacity");

            animal.EnclosureId = request.EnclosureId.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
            animal.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Specie))
            animal.Specie = request.Specie;

        if (!string.IsNullOrWhiteSpace(request.ImageUrl))
            animal.ImageUrl = request.ImageUrl;

        if (request.DateOfBirth.HasValue)
            animal.DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth.Value);

        if (request.Gender.HasValue)
            animal.Gender = request.Gender.Value;

        if (request.ArrivalDate.HasValue)
            animal.ArrivalDate = request.ArrivalDate.Value;

        await _context.SaveChangesAsync();

        var updatedAnimal = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .FirstOrDefaultAsync(a => a.Id == id);

        return updatedAnimal!.Adapt<AnimalResponseDto>();
    }

    public async Task<bool> DeleteAnimalAsync(int id)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal == null)
            return false;

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAnimalsByCategoryAsync(int categoryId)
    {
        var animals = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .Where(a => a.CategoryId == categoryId)
            .ToListAsync();

        return animals.Adapt<List<AnimalResponseDto>>();
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAnimalsByEnclosureAsync(int enclosureId)
    {
        var animals = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .Where(a => a.EnclosureId == enclosureId)
            .ToListAsync();

        return animals.Adapt<List<AnimalResponseDto>>();
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAnimalsBySpecieAsync(string specie)
    {
        if (string.IsNullOrWhiteSpace(specie))
            throw new ArgumentException("Specie parameter is required");

        var animals = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .Where(a => a.Specie.Contains(specie))
            .ToListAsync();

        return animals.Adapt<List<AnimalResponseDto>>();
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAnimalsByGenderAsync(Gender gender)
    {
        var animals = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .Where(a => a.Gender == gender)
            .ToListAsync();

        return animals.Adapt<List<AnimalResponseDto>>();
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAnimalsByAgeRangeAsync(int? minAge, int? maxAge)
    {
        var animals = await _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .Where(a => a.DateOfBirth.HasValue)
            .ToListAsync();

        animals = animals.Where(a =>
        {
            var age = DateTime.Now.Year - a.DateOfBirth!.Value.Year;
            if (minAge.HasValue && age < minAge.Value) return false;
            if (maxAge.HasValue && age > maxAge.Value) return false;
            return true;
        }).ToList();

        return animals.Adapt<List<AnimalResponseDto>>();
    }

    public async Task<IEnumerable<AnimalResponseDto>> GetAnimalsByArrivalDateAsync(DateTime? from, DateTime? to)
    {
        var query = _context.Animals
            .Include(a => a.Category)
            .Include(a => a.Enclosure)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(a => a.ArrivalDate >= from.Value);

        if (to.HasValue)
            query = query.Where(a => a.ArrivalDate <= to.Value);

        var animals = await query.ToListAsync();
        return animals.Adapt<List<AnimalResponseDto>>();
    }
}