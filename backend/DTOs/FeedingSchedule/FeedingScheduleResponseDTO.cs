using backend.DTOs.Animal;
using backend.DTOs.Staff;
using backend.Enums;

namespace backend.DTOs.FeedingSchedule;

public class FeedingScheduleResponseDto
{
    public int Id { get; set; }
    public AnimalSummaryDto Animal { get; set; } = null!;
    public StaffSummaryDto Staff { get; set; } = null!;
    public string? FoodType { get; set; }
    public decimal? QuantityInKg { get; set; }
    public DateTime FeedingTime { get; set; }
    public FeedingStatus Status { get; set; } = FeedingStatus.Pending;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}