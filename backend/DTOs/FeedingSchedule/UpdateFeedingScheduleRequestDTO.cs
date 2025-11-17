using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.FeedingSchedule;

public class UpdateFeedingScheduleRequestDto
{
    public int? AnimalId { get; set; }

    public int? StaffId { get; set; }

    [StringLength(100, ErrorMessage = "Food type cannot exceed 100 characters")]
    public string? FoodType { get; set; } 

    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal? Quantity { get; set; }

    public DateTime? FeedingTime { get; set; }

    public FeedingStatus? Status { get; set; }

    public string? Notes { get; set; }
}