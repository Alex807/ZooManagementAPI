using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.MedicalRecord;

public class UpdateMedicalRecordRequestDto
{
    public HealthStatus? HealthStatus { get; set; }

    public string? Description { get; set; }
}