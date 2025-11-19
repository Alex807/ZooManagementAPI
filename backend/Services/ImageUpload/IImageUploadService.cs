namespace backend.Services.ImageUpload;

public interface IImageUploadService
{
    // Uploads an image to ImgBB and returns the URL
    // Base64 encoded image string
    // Optional name for the image
    Task<string> UploadImageAsync(string imageBase64, string? imageName = null);

    // Uploads an image from IFormFile to ImgBB and returns the URL
    // The image file from the request
    Task<string> UploadImageAsync(IFormFile imageFile);

    // Validates if the file is a valid image format
    // The image file to validate
    // True if valid, false otherwise
    bool ValidateImageFile(IFormFile imageFile);
}
