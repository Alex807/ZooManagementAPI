namespace backend.DTOs.ImageUpload;

public class ImageUploadResponseDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public string Message { get; set; } = "Image uploaded successfully";
}
