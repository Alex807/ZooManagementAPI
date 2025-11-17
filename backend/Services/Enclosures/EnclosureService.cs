using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.Models;
using backend.DTOs.Enclosure;

namespace backend.Services.Enclosures;

public class EnclosureService : IEnclosureService
{
    private readonly ZooManagementDbContext _context;

    public EnclosureService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetAllEnclosuresAsync(EnclosureQueryDto query)
    {
        var dbQuery = _context.Enclosures
            .Include(e => e.Animals)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            dbQuery = dbQuery.Where(e => e.Name.Contains(query.Name));

        if (!string.IsNullOrWhiteSpace(query.Type))
            dbQuery = dbQuery.Where(e => e.Type != null && e.Type.Contains(query.Type));

        if (!string.IsNullOrWhiteSpace(query.Location))
            dbQuery = dbQuery.Where(e => e.Location != null && e.Location.Contains(query.Location));

        if (query.MinCapacity.HasValue)
            dbQuery = dbQuery.Where(e => e.Capacity >= query.MinCapacity.Value);

        if (query.MaxCapacity.HasValue)
            dbQuery = dbQuery.Where(e => e.Capacity <= query.MaxCapacity.Value);

        var enclosures = await dbQuery.ToListAsync();

        if (query.AtCapacity.HasValue)
            enclosures = enclosures.Where(e => (e.Animals.Count >= e.Capacity) == query.AtCapacity.Value).ToList();

        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<EnclosureResponseDto?> GetEnclosureByIdAsync(int id)
    {
        var enclosure = await _context.Enclosures
            .Include(e => e.Animals)
            .FirstOrDefaultAsync(e => e.Id == id);

        return enclosure?.Adapt<EnclosureResponseDto>();
    }

    public async Task<EnclosureResponseDto> CreateEnclosureAsync(CreateEnclosureRequestDto request)
    {
        var existingEnclosure = await _context.Enclosures
            .FirstOrDefaultAsync(e => e.Name == request.Name);

        if (existingEnclosure != null)
            throw new InvalidOperationException($"Enclosure with name '{request.Name}' already exists");

        var enclosure = new Enclosure
        {
            Name = request.Name,
            Type = request.Type,
            Capacity = request.Capacity,
            Location = request.Location
        };

        _context.Enclosures.Add(enclosure);
        await _context.SaveChangesAsync();

        var createdEnclosure = await _context.Enclosures
            .Include(e => e.Animals)
            .FirstOrDefaultAsync(e => e.Id == enclosure.Id);

        return createdEnclosure!.Adapt<EnclosureResponseDto>();
    }

    public async Task<EnclosureResponseDto?> UpdateEnclosureAsync(int id, UpdateEnclosureRequestDto request)
    {
        var enclosure = await _context.Enclosures
            .Include(e => e.Animals)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (enclosure == null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var existingEnclosure = await _context.Enclosures
                .FirstOrDefaultAsync(e => e.Name == request.Name && e.Id != id);

            if (existingEnclosure != null)
                throw new InvalidOperationException($"Enclosure with name '{request.Name}' already exists");

            enclosure.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Type))
            enclosure.Type = request.Type;

        if (request.Capacity.HasValue)
        {
            if (request.Capacity.Value < enclosure.Animals.Count)
                throw new InvalidOperationException($"Cannot set capacity to {request.Capacity.Value} because enclosure currently has {enclosure.Animals.Count} animals");

            enclosure.Capacity = request.Capacity.Value;
        }

        if (request.Location != null)
            enclosure.Location = request.Location;

        await _context.SaveChangesAsync();

        var updatedEnclosure = await _context.Enclosures
            .Include(e => e.Animals)
            .FirstOrDefaultAsync(e => e.Id == id);

        return updatedEnclosure!.Adapt<EnclosureResponseDto>();
    }

    public async Task<bool> DeleteEnclosureAsync(int id)
    {
        var enclosure = await _context.Enclosures
            .Include(e => e.Animals)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (enclosure == null)
            return false;

        if (enclosure.Animals.Any())
            throw new InvalidOperationException($"Cannot delete enclosure '{enclosure.Name}' because it has {enclosure.Animals.Count} animals");

        _context.Enclosures.Remove(enclosure);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name parameter is required");

        var enclosures = await _context.Enclosures
            .Include(e => e.Animals)
            .Where(e => e.Name.Contains(name))
            .ToListAsync();

        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByTypeAsync(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type parameter is required");

        var enclosures = await _context.Enclosures
            .Include(e => e.Animals)
            .Where(e => e.Type != null && e.Type.Contains(type))
            .ToListAsync();

        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByLocationAsync(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location parameter is required");

        var enclosures = await _context.Enclosures
            .Include(e => e.Animals)
            .Where(e => e.Location != null && e.Location.Contains(location))
            .ToListAsync();

        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByCapacityAsync(int? min, int? max)
    {
        var query = _context.Enclosures
            .Include(e => e.Animals)
            .AsQueryable();

        if (min.HasValue)
            query = query.Where(e => e.Capacity >= min.Value);

        if (max.HasValue)
            query = query.Where(e => e.Capacity <= max.Value);

        var enclosures = await query.ToListAsync();
        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresAtCapacityAsync()
    {
        var enclosures = await _context.Enclosures
            .Include(e => e.Animals)
            .ToListAsync();

        enclosures = enclosures.Where(e => e.Animals.Count >= e.Capacity).ToList();
        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetAvailableEnclosuresAsync()
    {
        var enclosures = await _context.Enclosures
            .Include(e => e.Animals)
            .ToListAsync();

        enclosures = enclosures.Where(e => e.Animals.Count < e.Capacity).ToList();
        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }

    public async Task<IEnumerable<EnclosureResponseDto>> GetEmptyEnclosuresAsync()
    {
        var enclosures = await _context.Enclosures
            .Include(e => e.Animals)
            .Where(e => !e.Animals.Any())
            .ToListAsync();

        return enclosures.Adapt<List<EnclosureResponseDto>>();
    }
}