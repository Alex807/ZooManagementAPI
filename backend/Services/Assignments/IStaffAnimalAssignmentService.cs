using backend.DTOs.Assignment;

namespace backend.Services.Assignments;

public interface IStaffAnimalAssignmentService
{
    Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAllAssignmentsAsync(StaffAnimalAssignmentQueryDto query);
    Task<StaffAnimalAssignmentResponseDto?> GetAssignmentByIdAsync(int staffId, int animalId);
    Task<StaffAnimalAssignmentResponseDto> CreateAssignmentAsync(CreateAssignmentRequestDto request);
    Task<StaffAnimalAssignmentResponseDto?> UpdateAssignmentAsync(int staffId, int animalId, UpdateAssignmentRequestDto request);
    Task<bool> DeleteAssignmentAsync(int staffId, int animalId);
    Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsByStaffAsync(int staffId);
    Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsByAnimalAsync(int animalId);
    Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsWithObservationsAsync();
    Task<IEnumerable<StaffAnimalAssignmentResponseDto>> GetAssignmentsByDateRangeAsync(DateTime? from, DateTime? to);
}