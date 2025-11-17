using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Enclosure;

public class CreateEnclosureRequestDto
{
    [Required(ErrorMessage = "Enclosure name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
    public string? Type { get; set; }

    [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100")]
    public int Capacity { get; set; }

    [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters")]
    public string? Location { get; set; }
}