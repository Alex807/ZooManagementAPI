using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Staff;

public class CreateStaffRequestDto
{
    [Required(ErrorMessage = "User account ID is required")]
    public int UserAccountId { get; set; }

    [Required(ErrorMessage = "Hire date is required")]
    public DateTime HireDate { get; set; }

    [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
    public string? Department { get; set; }

    [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
    public string? Position { get; set; }

    [Range(0, 999999.99, ErrorMessage = "Salary must be between 0 and 999999.99")]
    public decimal? Salary { get; set; }
}