using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.MedicalRecord;

public class CreateMedicalRecordRequestDto
{
    [Required(ErrorMessage = "Animal ID is required")]
    public int AnimalId { get; set; }

    [Required(ErrorMessage = "Staff ID (Veterinarian) is required")]
    public int StaffId { get; set; }

    [Required(ErrorMessage = "Health status is required")]
    public HealthStatus HealthStatus { get; set; }

    public string? Description { get; set; }
}