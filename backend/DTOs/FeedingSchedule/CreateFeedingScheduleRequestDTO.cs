using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.FeedingSchedule;

public class CreateFeedingScheduleRequestDto
{
    [Required(ErrorMessage = "Animal ID is required")]
    public int AnimalId { get; set; }

    [Required(ErrorMessage = "Staff ID is required")]
    public int StaffId { get; set; }

    [Required(ErrorMessage = "Food type is required")]
    [StringLength(100, ErrorMessage = "Food type cannot exceed 100 characters")]
    public string FoodType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity in Kg is required")] 
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity in Kg must be greater than 0")]
    public decimal QuantityInKg { get; set; }

    [Required(ErrorMessage = "Feeding time is required")]
    public DateTime FeedingTime { get; set; }

    [Required(ErrorMessage = "Feeding status is required")]
    public FeedingStatus Status { get; set; } = FeedingStatus.Pending;

    public string? Notes { get; set; }
}