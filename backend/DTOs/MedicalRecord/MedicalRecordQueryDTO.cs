using backend.Enums;

namespace backend.DTOs.MedicalRecord;

public class MedicalRecordQueryDto
{
    public int? AnimalId { get; set; }
    public int? StaffId { get; set; }
    public HealthStatus? Status { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}