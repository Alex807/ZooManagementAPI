using backend.DTOs.MedicalRecord;
using backend.Enums;

namespace backend.Services.MedicalRecords;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordResponseDto>> GetAllMedicalRecordsAsync(MedicalRecordQueryDto query);
    Task<MedicalRecordResponseDto?> GetMedicalRecordByIdAsync(int id);
    Task<MedicalRecordResponseDto> CreateMedicalRecordAsync(CreateMedicalRecordRequestDto request);
    Task<MedicalRecordResponseDto?> UpdateMedicalRecordAsync(int id, UpdateMedicalRecordRequestDto request);
    Task<bool> DeleteMedicalRecordAsync(int id);
    Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByAnimalAsync(int animalId);
    Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByStaffAsync(int staffId);
    Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByStatusAsync(HealthStatus status);
    Task<IEnumerable<MedicalRecordResponseDto>> GetMedicalRecordsByDateRangeAsync(DateTime? from, DateTime? to);
    Task<IEnumerable<MedicalRecordResponseDto>> GetRecentMedicalRecordsAsync(int days);
}