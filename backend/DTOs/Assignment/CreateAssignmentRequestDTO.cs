using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Assignment;

public class CreateAssignmentRequestDto
{
    [Required(ErrorMessage = "Animal ID is required")]
    public int AnimalId { get; set; }

    [Required(ErrorMessage = "Staff ID is required")]
    public int StaffId { get; set; }

    [StringLength(200, ErrorMessage = "Observations cannot exceed 200 characters")]
    public string? Observations { get; set; }
}