using backend.DTOs.Staff;

namespace backend.Services.Staff;

public interface IStaffService
{
    Task<IEnumerable<StaffResponseDto>> GetAllStaffAsync(StaffQueryDto query);
    Task<StaffResponseDto?> GetStaffByIdAsync(int id);
    Task<StaffResponseDto> CreateStaffAsync(CreateStaffRequestDto request);
    Task<StaffResponseDto?> UpdateStaffAsync(int id, UpdateStaffRequestDto request);
    Task<bool> DeleteStaffAsync(int id);
    Task<IEnumerable<StaffResponseDto>> GetStaffByDepartmentAsync(string department);
    Task<IEnumerable<StaffResponseDto>> GetStaffByPositionAsync(string position);
    Task<IEnumerable<StaffResponseDto>> GetStaffBySalaryRangeAsync(decimal? min, decimal? max);
    Task<IEnumerable<StaffResponseDto>> GetStaffByHireDateAsync(DateTime? from, DateTime? to);
    Task<StaffResponseDto?> GetStaffByUserAccountIdAsync(int userAccountId);
}