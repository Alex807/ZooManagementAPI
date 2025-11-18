using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.Models;
using backend.DTOs.FeedingSchedule;
using backend.Enums;

namespace backend.Services.FeedingSchedules;

public class FeedingScheduleService : IFeedingScheduleService
{
    private readonly ZooManagementDbContext _context;

    public FeedingScheduleService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetAllFeedingSchedulesAsync(FeedingScheduleQueryDto query)
    {
        var dbQuery = _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (query.AnimalId.HasValue)
            dbQuery = dbQuery.Where(f => f.AnimalId == query.AnimalId.Value);

        if (query.StaffId.HasValue)
            dbQuery = dbQuery.Where(f => f.StaffId == query.StaffId.Value);

        if (!string.IsNullOrWhiteSpace(query.FoodType))
            dbQuery = dbQuery.Where(f => f.FoodType.Contains(query.FoodType));

        if (query.Status.HasValue)
            dbQuery = dbQuery.Where(f => f.Status == query.Status.Value);

        if (query.FeedingTimeFrom.HasValue)
            dbQuery = dbQuery.Where(f => f.FeedingTime >= query.FeedingTimeFrom.Value);

        if (query.FeedingTimeTo.HasValue)
            dbQuery = dbQuery.Where(f => f.FeedingTime <= query.FeedingTimeTo.Value);

        var schedules = await dbQuery.OrderBy(f => f.FeedingTime).ToListAsync();
        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<FeedingScheduleResponseDto?> GetFeedingScheduleByIdAsync(int id)
    {
        var schedule = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(f => f.Id == id);

        return schedule?.Adapt<FeedingScheduleResponseDto>();
    }

    public async Task<FeedingScheduleResponseDto> CreateFeedingScheduleAsync(CreateFeedingScheduleRequestDto request)
    {
        var animalExists = await _context.Animals.AnyAsync(a => a.Id == request.AnimalId);
        if (!animalExists)
            throw new InvalidOperationException($"Animal with ID {request.AnimalId} does not exist");

        var staffExists = await _context.Staff.AnyAsync(s => s.Id == request.StaffId);
        if (!staffExists)
            throw new InvalidOperationException($"Staff with ID {request.StaffId} does not exist");

        var schedule = new FeedingSchedule
        {
            AnimalId = request.AnimalId,
            StaffId = request.StaffId,
            FoodType = request.FoodType,
            QuantityInKg = request.QuantityInKg,
            FeedingTime = request.FeedingTime,
            Status = request.Status,
            Notes = request.Notes
        };

        _context.FeedingSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        var createdSchedule = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(f => f.Id == schedule.Id);

        return createdSchedule!.Adapt<FeedingScheduleResponseDto>();
    }

    public async Task<FeedingScheduleResponseDto?> UpdateFeedingScheduleAsync(int id, UpdateFeedingScheduleRequestDto request)
    {
        var schedule = await _context.FeedingSchedules.FindAsync(id);
        if (schedule == null)
            return null;

        if (request.AnimalId.HasValue)
        {
            var animalExists = await _context.Animals.AnyAsync(a => a.Id == request.AnimalId.Value);
            if (!animalExists)
                throw new InvalidOperationException($"Animal with ID {request.AnimalId.Value} does not exist");
            schedule.AnimalId = request.AnimalId.Value;
        }

        if (request.StaffId.HasValue)
        {
            var staffExists = await _context.Staff.AnyAsync(s => s.Id == request.StaffId.Value);
            if (!staffExists)
                throw new InvalidOperationException($"Staff with ID {request.StaffId.Value} does not exist");
            schedule.StaffId = request.StaffId.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.FoodType))
            schedule.FoodType = request.FoodType;

        if (request.Quantity.HasValue)
            schedule.QuantityInKg = request.Quantity.Value;

        if (request.FeedingTime.HasValue)
            schedule.FeedingTime = request.FeedingTime.Value;

        if (request.Status.HasValue)
            schedule.Status = request.Status.Value;

        if (request.Notes != null)
            schedule.Notes = request.Notes;

        await _context.SaveChangesAsync();

        var updatedSchedule = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(f => f.Id == id);

        return updatedSchedule!.Adapt<FeedingScheduleResponseDto>();
    }

    public async Task<bool> DeleteFeedingScheduleAsync(int id)
    {
        var schedule = await _context.FeedingSchedules.FindAsync(id);
        if (schedule == null)
            return false;

        _context.FeedingSchedules.Remove(schedule);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByAnimalAsync(int animalId)
    {
        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.AnimalId == animalId)
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByStaffAsync(int staffId)
    {
        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.StaffId == staffId)
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByStatusAsync(FeedingStatus status)
    {
        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.Status == status)
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByFoodTypeAsync(string foodType)
    {
        if (string.IsNullOrWhiteSpace(foodType))
            throw new ArgumentException("Food type parameter is required");

        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.FoodType.Contains(foodType))
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByDateRangeAsync(DateTime? from, DateTime? to)
    {
        var query = _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(f => f.FeedingTime >= from.Value);

        if (to.HasValue)
            query = query.Where(f => f.FeedingTime <= to.Value);

        var schedules = await query.OrderBy(f => f.FeedingTime).ToListAsync();
        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetTodaysFeedingSchedulesAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.FeedingTime >= today && f.FeedingTime < tomorrow)
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetUpcomingFeedingSchedulesAsync(int hours)
    {
        var now = DateTime.UtcNow;
        var cutoffTime = now.AddHours(hours);

        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.FeedingTime >= now && f.FeedingTime <= cutoffTime && f.Status == FeedingStatus.Pending)
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }

    public async Task<IEnumerable<FeedingScheduleResponseDto>> GetOverdueFeedingSchedulesAsync()
    {
        var now = DateTime.UtcNow;

        var schedules = await _context.FeedingSchedules
            .Include(f => f.Animal)
            .Include(f => f.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(f => f.FeedingTime < now && f.Status == FeedingStatus.Pending)
            .OrderBy(f => f.FeedingTime)
            .ToListAsync();

        return schedules.Adapt<List<FeedingScheduleResponseDto>>();
    }
}