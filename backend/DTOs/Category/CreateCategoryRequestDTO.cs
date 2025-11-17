using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Category;

public class CreateCategoryRequestDto
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(2048, ErrorMessage = "Image URL cannot exceed 2048 characters")]
    public string ImageUrl { get; set; } = string.Empty;
}