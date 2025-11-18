using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.Models;
using backend.DTOs.Assignment;

namespace backend.Services.Assignments;

public class StaffAnimalAssignmentService : IStaffAnimalAssignmentService
{
    private readonly ZooManagementDbContext _context;

    public StaffAnimalAssignmentService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAllAssignmentsAsync(StaffAnimalAssignmentQueryDto query)
    {
        var dbQuery = _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .AsQueryable();

        if (query.StaffId.HasValue)
            dbQuery = dbQuery.Where(a => a.StaffId == query.StaffId.Value);

        if (query.AnimalId.HasValue)
            dbQuery = dbQuery.Where(a => a.AnimalId == query.AnimalId.Value);

        var assignments = await dbQuery.ToListAsync();
        return assignments.Adapt<List<StaffAnimalAssignmentResponseDto>>();
    }

    public async Task<StaffAnimalAssignmentResponseDto?> GetAssignmentByIdAsync(int staffId, int animalId)
    {
        var assignment = await _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .FirstOrDefaultAsync(a => a.StaffId == staffId && a.AnimalId == animalId);

        return assignment?.Adapt<StaffAnimalAssignmentResponseDto>();
    }

    public async Task<StaffAnimalAssignmentResponseDto> CreateAssignmentAsync(CreateAssignmentRequestDto request)
    {
        var staffExists = await _context.Staff.AnyAsync(s => s.Id == request.StaffId);
        if (!staffExists)
            throw new InvalidOperationException($"Staff with ID {request.StaffId} does not exist");

        var animalExists = await _context.Animals.AnyAsync(a => a.Id == request.AnimalId);
        if (!animalExists)
            throw new InvalidOperationException($"Animal with ID {request.AnimalId} does not exist");

        var existingAssignment = await _context.StaffAnimalAssignments
            .FirstOrDefaultAsync(a => a.StaffId == request.StaffId && a.AnimalId == request.AnimalId);

        if (existingAssignment != null)
            throw new InvalidOperationException($"Assignment for Staff ID {request.StaffId} and Animal ID {request.AnimalId} already exists");

        var assignment = new StaffAnimalAssignment
        {
            StaffId = request.StaffId,
            AnimalId = request.AnimalId,
            Observations = request.Observations
        };

        _context.StaffAnimalAssignments.Add(assignment);
        await _context.SaveChangesAsync();

        var createdAssignment = await _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .FirstOrDefaultAsync(a => a.StaffId == request.StaffId && a.AnimalId == request.AnimalId);

        return createdAssignment!.Adapt<StaffAnimalAssignmentResponseDto>();
    }

    public async Task<StaffAnimalAssignmentResponseDto?> UpdateAssignmentAsync(int staffId, int animalId, UpdateAssignmentRequestDto request)
    {
        var assignment = await _context.StaffAnimalAssignments
            .FirstOrDefaultAsync(a => a.StaffId == staffId && a.AnimalId == animalId);

        if (assignment == null)
            return null;

        if (request.StaffId.HasValue || request.AnimalId.HasValue)
        {
            var newStaffId = request.StaffId ?? staffId;
            var newAnimalId = request.AnimalId ?? animalId;

            if (newStaffId != staffId || newAnimalId != animalId)
            {
                if (request.StaffId.HasValue)
                {
                    var staffExists = await _context.Staff.AnyAsync(s => s.Id == request.StaffId.Value);
                    if (!staffExists)
                        throw new InvalidOperationException($"Staff with ID {request.StaffId.Value} does not exist");
                }

                if (request.AnimalId.HasValue)
                {
                    var animalExists = await _context.Animals.AnyAsync(a => a.Id == request.AnimalId.Value);
                    if (!animalExists)
                        throw new InvalidOperationException($"Animal with ID {request.AnimalId.Value} does not exist");
                }

                var existingAssignment = await _context.StaffAnimalAssignments
                    .FirstOrDefaultAsync(a => a.StaffId == newStaffId && a.AnimalId == newAnimalId);

                if (existingAssignment != null)
                    throw new InvalidOperationException($"Assignment for Staff ID {newStaffId} and Animal ID {newAnimalId} already exists");

                _context.StaffAnimalAssignments.Remove(assignment);

                var newAssignment = new StaffAnimalAssignment
                {
                    StaffId = newStaffId,
                    AnimalId = newAnimalId,
                    Observations = request.Observations ?? assignment.Observations
                };

                _context.StaffAnimalAssignments.Add(newAssignment);
                await _context.SaveChangesAsync();

                var updatedAssignment = await _context.StaffAnimalAssignments
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.UserAccount)
                            .ThenInclude(u => u.UserDetails)
                    .Include(a => a.Animal)
                    .FirstOrDefaultAsync(a => a.StaffId == newStaffId && a.AnimalId == newAnimalId);

                return updatedAssignment!.Adapt<StaffAnimalAssignmentResponseDto>();
            }
        }

        if (request.Observations != null)
            assignment.Observations = request.Observations;

        _context.Entry(assignment).Property(a => a.UpdatedAt).CurrentValue = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var finalAssignment = await _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .FirstOrDefaultAsync(a => a.StaffId == staffId && a.AnimalId == animalId);

        return finalAssignment!.Adapt<StaffAnimalAssignmentResponseDto>();
    }

    public async Task<bool> DeleteAssignmentAsync(int staffId, int animalId)
    {
        var assignment = await _context.StaffAnimalAssignments
            .FirstOrDefaultAsync(a => a.StaffId == staffId && a.AnimalId == animalId);

        if (assignment == null)
            return false;

        _context.StaffAnimalAssignments.Remove(assignment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsByStaffAsync(int staffId)
    {
        var assignments = await _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .Where(a => a.StaffId == staffId)
            .ToListAsync();

        return assignments.Adapt<List<StaffAnimalAssignmentResponseDto>>();
    }

    public async Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsByAnimalAsync(int animalId)
    {
        var assignments = await _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .Where(a => a.AnimalId == animalId)
            .ToListAsync();

        return assignments.Adapt<List<StaffAnimalAssignmentResponseDto>>();
    }

    public async Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsWithObservationsAsync()
    {
        var assignments = await _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .Where(a => !string.IsNullOrEmpty(a.Observations))
            .ToListAsync();

        return assignments.Adapt<List<StaffAnimalAssignmentResponseDto>>();
    }

    public async Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsByDateRangeAsync(DateTime? from, DateTime? to)
    {
        var query = _context.StaffAnimalAssignments
            .Include(a => a.Staff)
                .ThenInclude(s => s.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Include(a => a.Animal)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(a => a.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(a => a.CreatedAt <= to.Value);

        var assignments = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        return assignments.Adapt<List<StaffAnimalAssignmentResponseDto>>();
    }
}