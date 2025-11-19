using System.Text.Json;

namespace backend.Services.ImageUpload;

public class ImageUploadService : IImageUploadService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<ImageUploadService> _logger;
    private const string ImgBBApiUrl = "https://api.imgbb.com/1/upload";
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];
    private const long MaxFileSize = 32 * 1024 * 1024; // 32MB - ImgBB limit

    public ImageUploadService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ImageUploadService> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiKey = Environment.GetEnvironmentVariable("ImgBB__KEY", EnvironmentVariableTarget.User)
                  ?? throw new InvalidOperationException("ImgBB__KEY environment variable not found");
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(string imageBase64, string? imageName = null)
    {
        try
        {
            // Remove data URI prefix if present (e.g., "data:image/png;base64,")
            if (imageBase64.Contains(","))
            {
                imageBase64 = imageBase64.Split(',')[1];
            }

            var formData = new MultipartFormDataContent
            {
                { new StringContent(_apiKey), "key" },
                { new StringContent(imageBase64), "image" }
            };

            if (!string.IsNullOrWhiteSpace(imageName))
            {
                formData.Add(new StringContent(imageName), "name");
            }

            var response = await _httpClient.PostAsync(ImgBBApiUrl, formData);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("ImgBB API error: {StatusCode} - {Response}", response.StatusCode, responseContent);
                throw new InvalidOperationException($"Failed to upload image to ImgBB: {response.StatusCode}");
            }

            var jsonResponse = JsonDocument.Parse(responseContent);
            var imageUrl = jsonResponse.RootElement
                .GetProperty("data")
                .GetProperty("url")
                .GetString();

            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new InvalidOperationException("ImgBB did not return a valid image URL");
            }

            _logger.LogInformation("Image uploaded successfully to ImgBB: {Url}", imageUrl);
            return imageUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image to ImgBB");
            throw new InvalidOperationException("Failed to upload image to cloud storage", ex);
        }
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("Image file is required");
        }

        if (!ValidateImageFile(imageFile))
        {
            throw new ArgumentException("Invalid image file format or size");
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            var imageBase64 = Convert.ToBase64String(imageBytes);

            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            return await UploadImageAsync(imageBase64, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing image file for upload");
            throw new InvalidOperationException("Failed to process image file", ex);
        }
    }

    public bool ValidateImageFile(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            _logger.LogWarning("Image file is null or empty");
            return false;
        }

        if (imageFile.Length > MaxFileSize)
        {
            _logger.LogWarning("Image file size {Size} exceeds maximum allowed size {MaxSize}", imageFile.Length, MaxFileSize);
            return false;
        }

        var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            _logger.LogWarning("Image file extension {Extension} is not allowed", extension);
            return false;
        }

        // Additional validation: check content type
        if (!imageFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Image file content type {ContentType} is not valid", imageFile.ContentType);
            return false;
        }

        return true;
    }
}
