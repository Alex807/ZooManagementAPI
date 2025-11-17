using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Assignment;

public class UpdateAssignmentRequestDto
{
    public int? AnimalId { get; set; }
    public int? StaffId { get; set; }

    [StringLength(200, ErrorMessage = "Observations cannot exceed 200 characters")]
    public string? Observations { get; set; }
}