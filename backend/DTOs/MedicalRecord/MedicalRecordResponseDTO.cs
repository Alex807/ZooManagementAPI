using backend.DTOs.Animal;
using backend.DTOs.Staff;
using backend.Enums;

namespace backend.DTOs.MedicalRecord;

public class MedicalRecordResponseDto
{
    public int Id { get; set; }
    public AnimalSummaryDto Animal { get; set; } = null!;
    public StaffSummaryDto Veterinarian { get; set; } = null!;
    public HealthStatus HealthStatus { get; set; }
    public string? Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}