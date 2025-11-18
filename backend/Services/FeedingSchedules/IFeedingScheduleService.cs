using backend.DTOs.FeedingSchedule;
using backend.Enums;

namespace backend.Services.FeedingSchedules;

public interface IFeedingScheduleService
{
    Task<IEnumerable<FeedingScheduleResponseDto>> GetAllFeedingSchedulesAsync(FeedingScheduleQueryDto query);
    Task<FeedingScheduleResponseDto?> GetFeedingScheduleByIdAsync(int id);
    Task<FeedingScheduleResponseDto> CreateFeedingScheduleAsync(CreateFeedingScheduleRequestDto request);
    Task<FeedingScheduleResponseDto?> UpdateFeedingScheduleAsync(int id, UpdateFeedingScheduleRequestDto request);
    Task<bool> DeleteFeedingScheduleAsync(int id);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByAnimalAsync(int animalId);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByStaffAsync(int staffId);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByStatusAsync(FeedingStatus status);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByFoodTypeAsync(string foodType);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetFeedingSchedulesByDateRangeAsync(DateTime? from, DateTime? to);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetTodaysFeedingSchedulesAsync();
    Task<IEnumerable<FeedingScheduleResponseDto>> GetUpcomingFeedingSchedulesAsync(int hours);
    Task<IEnumerable<FeedingScheduleResponseDto>> GetOverdueFeedingSchedulesAsync();
}