using backend.Enums;

namespace backend.DTOs.FeedingSchedule;

public class FeedingScheduleQueryDto
{
    public int? AnimalId { get; set; }
    public int? StaffId { get; set; }
    public string? FoodType { get; set; }
    public FeedingStatus? Status { get; set; }
    public DateTime? FeedingTimeFrom { get; set; }
    public DateTime? FeedingTimeTo { get; set; }
}