using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.ImageUpload;

public class ImageUploadBase64RequestDto
{
    [Required(ErrorMessage = "Image data is required")]
    public string ImageBase64 { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ImageName { get; set; }
}
