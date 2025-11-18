using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.Models;
using backend.DTOs.MedicalRecord;
using backend.Enums;

namespace backend.Services.MedicalRecords;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly ZooManagementDbContext _context;

    public MedicalRecordService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedicalRecordResponseDto>> GetAllMedicalRecordsAsync(MedicalRecordQueryDto query)
    {
        var dbQuery = _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (query.AnimalId.HasValue)
            dbQuery = dbQuery.Where(m => m.AnimalId == query.AnimalId.Value);

        if (query.StaffId.HasValue)
            dbQuery = dbQuery.Where(m => m.StaffId == query.StaffId.Value);

        if (query.Status.HasValue)
            dbQuery = dbQuery.Where(m => m.Status == query.Status.Value);

        if (query.CreatedAfter.HasValue)
            dbQuery = dbQuery.Where(m => m.CreatedAt >= query.CreatedAfter.Value);

        if (query.CreatedBefore.HasValue)
            dbQuery = dbQuery.Where(m => m.CreatedAt <= query.CreatedBefore.Value);

        var records = await dbQuery.ToListAsync();
        return records.Adapt<List<MedicalRecordResponseDto>>();
    }

    public async Task<MedicalRecordResponseDto?> GetMedicalRecordByIdAsync(int id)
    {
        var record = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(m => m.Id == id);

        return record?.Adapt<MedicalRecordResponseDto>();
    }

    public async Task<MedicalRecordResponseDto> CreateMedicalRecordAsync(CreateMedicalRecordRequestDto request)
    {
        var animalExists = await _context.Animals.AnyAsync(a => a.Id == request.AnimalId);
        if (!animalExists)
            throw new InvalidOperationException($"Animal with ID {request.AnimalId} does not exist");

        var staffExists = await _context.Staff.AnyAsync(s => s.Id == request.StaffId);
        if (!staffExists)
            throw new InvalidOperationException($"Staff with ID {request.StaffId} does not exist");

        var record = new MedicalRecord
        {
            AnimalId = request.AnimalId,
            StaffId = request.StaffId,
            Status = request.HealthStatus,
            Description = request.Description
        };

        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();

        var createdRecord = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(m => m.Id == record.Id);

        return createdRecord!.Adapt<MedicalRecordResponseDto>();
    }

    public async Task<MedicalRecordResponseDto?> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordRequestDto request)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        if (record == null)
            return null;

        if (request.HealthStatus.HasValue)
            record.Status = request.HealthStatus.Value;

        if (request.Description != null)
            record.Description = request.Description;

        await _context.SaveChangesAsync();

        var updatedRecord = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .FirstOrDefaultAsync(m => m.Id == id);

        return updatedRecord!.Adapt<MedicalRecordResponseDto>();
    }

    public async Task<bool> DeleteMedicalRecordAsync(int id)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        if (record == null)
            return false;

        _context.MedicalRecords.Remove(record);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByAnimalAsync(int animalId)
    {
        var records = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(m => m.AnimalId == animalId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return records.Adapt<List<MedicalRecordResponseDto>>();
    }

    public async Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByStaffAsync(int staffId)
    {
        var records = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(m => m.StaffId == staffId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return records.Adapt<List<MedicalRecordResponseDto>>();
    }

    public async Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByStatusAsync(HealthStatus status)
    {
        var records = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(m => m.Status == status)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return records.Adapt<List<MedicalRecordResponseDto>>();
    }

    public async Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByDateRangeAsync(DateTime? from, DateTime? to)
    {
        var query = _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(m => m.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(m => m.CreatedAt <= to.Value);

        var records = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        return records.Adapt<List<MedicalRecordResponseDto>>();
    }

    public async Task<IEnumerable<MedicalRecordResponseDto>> GetRecentMedicalRecordsAsync(int days)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);

        var records = await _context.MedicalRecords
            .Include(m => m.Animal)
            .Include(m => m.Staff)
                .ThenInclude(s => s!.UserAccount)
                    .ThenInclude(u => u.UserDetails)
            .Where(m => m.CreatedAt >= cutoffDate)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return records.Adapt<List<MedicalRecordResponseDto>>();
    }
}