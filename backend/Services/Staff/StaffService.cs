using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.DTOs.Staff;

namespace backend.Services.Staff;

public class StaffService : IStaffService
{
    private readonly ZooManagementDbContext _context;

    public StaffService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffResponseDto>> GetAllStaffAsync(StaffQueryDto query)
    {
        var dbQuery = _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Department))
            dbQuery = dbQuery.Where(s => s.Department.Contains(query.Department));

        if (!string.IsNullOrWhiteSpace(query.Position))
            dbQuery = dbQuery.Where(s => s.Position.Contains(query.Position));

        if (query.MinSalary.HasValue)
            dbQuery = dbQuery.Where(s => s.Salary >= query.MinSalary.Value);

        if (query.MaxSalary.HasValue)
            dbQuery = dbQuery.Where(s => s.Salary <= query.MaxSalary.Value);

        if (query.HiredAfter.HasValue)
            dbQuery = dbQuery.Where(s => s.HireDate >= query.HiredAfter.Value);

        if (query.HiredBefore.HasValue)
            dbQuery = dbQuery.Where(s => s.HireDate <= query.HiredBefore.Value);

        var staff = await dbQuery.ToListAsync();
        return staff.Adapt<List<StaffResponseDto>>();
    }

    public async Task<StaffResponseDto?> GetStaffByIdAsync(int id)
    {
        var staff = await _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(s => s.Id == id);

        return staff?.Adapt<StaffResponseDto>();
    }

    public async Task<StaffResponseDto> CreateStaffAsync(CreateStaffRequestDto request)
    {
        var userAccountExists = await _context.UserAccounts.AnyAsync(u => u.Id == request.UserAccountId);
        if (!userAccountExists)
            throw new InvalidOperationException($"User account with ID {request.UserAccountId} does not exist");

        var existingStaff = await _context.Staff
            .FirstOrDefaultAsync(s => s.UserAccountId == request.UserAccountId);

        if (existingStaff != null)
            throw new InvalidOperationException($"Staff record for user account ID {request.UserAccountId} already exists");

        var staff = new Models.Staff
        {
            UserAccountId = request.UserAccountId,
            HireDate = request.HireDate,
            Department = request.Department ?? string.Empty,
            Position = request.Position ?? string.Empty,
            Salary = request.Salary ?? 0
        };

        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        var createdStaff = await _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(s => s.Id == staff.Id);

        return createdStaff!.Adapt<StaffResponseDto>();
    }

    public async Task<StaffResponseDto?> UpdateStaffAsync(int id, UpdateStaffRequestDto request)
    {
        var staff = await _context.Staff.FindAsync(id);
        if (staff == null)
            return null;

        if (request.HireDate.HasValue)
            staff.HireDate = request.HireDate.Value;

        if (!string.IsNullOrWhiteSpace(request.Department))
            staff.Department = request.Department;

        if (!string.IsNullOrWhiteSpace(request.Position))
            staff.Position = request.Position;

        if (request.Salary.HasValue)
            staff.Salary = request.Salary.Value;

        await _context.SaveChangesAsync();

        var updatedStaff = await _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(s => s.Id == id);

        return updatedStaff!.Adapt<StaffResponseDto>();
    }

    public async Task<bool> DeleteStaffAsync(int id)
    {
        var staff = await _context.Staff
            .Include(s => s.FeedingSchedules)
            .Include(s => s.MedicalRecords)
            .Include(s => s.AnimalAssignments)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (staff == null)
            return false;

        if (staff.FeedingSchedules.Any() || staff.MedicalRecords.Any() || staff.AnimalAssignments.Any())
            throw new InvalidOperationException("Cannot delete staff member because they have associated records (feeding schedules, medical records, or animal assignments)");

        _context.Staff.Remove(staff);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StaffResponseDto>> GetStaffByDepartmentAsync(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Department parameter is required");

        var staff = await _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .Where(s => s.Department.Contains(department))
            .ToListAsync();

        return staff.Adapt<List<StaffResponseDto>>();
    }

    public async Task<IEnumerable<StaffResponseDto>> GetStaffByPositionAsync(string position)
    {
        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position parameter is required");

        var staff = await _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .Where(s => s.Position.Contains(position))
            .ToListAsync();

        return staff.Adapt<List<StaffResponseDto>>();
    }

    public async Task<IEnumerable<StaffResponseDto>> GetStaffBySalaryRangeAsync(decimal? min, decimal? max)
    {
        var query = _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (min.HasValue)
            query = query.Where(s => s.Salary >= min.Value);

        if (max.HasValue)
            query = query.Where(s => s.Salary <= max.Value);

        var staff = await query.ToListAsync();
        return staff.Adapt<List<StaffResponseDto>>();
    }

    public async Task<IEnumerable<StaffResponseDto>> GetStaffByHireDateAsync(DateTime? from, DateTime? to)
    {
        var query = _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(s => s.HireDate >= from.Value);

        if (to.HasValue)
            query = query.Where(s => s.HireDate <= to.Value);

        var staff = await query.ToListAsync();
        return staff.Adapt<List<StaffResponseDto>>();
    }

    public async Task<StaffResponseDto?> GetStaffByUserAccountIdAsync(int userAccountId)
    {
        var staff = await _context.Staff
            .Include(s => s.UserAccount)
                .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(s => s.UserAccountId == userAccountId);

        return staff?.Adapt<StaffResponseDto>();
    }
}