using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Category;

public class UpdateCategoryRequestDto
{
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(2048, ErrorMessage = "Image URL cannot exceed 2048 characters")]
    public string? ImageUrl { get; set; }
}