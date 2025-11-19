using Microsoft.AspNetCore.Mvc;
using backend.DTOs.ImageUpload;
using backend.Services.ImageUpload;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageUploadController : ControllerBase
{
    private readonly IImageUploadService _imageUploadService;
    private readonly ILogger<ImageUploadController> _logger;

    public ImageUploadController(IImageUploadService imageUploadService, ILogger<ImageUploadController> logger)
    {
        _imageUploadService = imageUploadService;
        _logger = logger;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ImageUploadResponseDto>> UploadImage([FromForm] IFormFile imageFile)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest(new { message = "No image file provided" });
            }

            if (!_imageUploadService.ValidateImageFile(imageFile))
            {
                return BadRequest(new { message = "Invalid image file. Allowed formats: jpg, jpeg, png, gif, bmp, webp. Maximum size: 32MB" });
            }

            var imageUrl = await _imageUploadService.UploadImageAsync(imageFile);

            return Ok(new ImageUploadResponseDto
            {
                ImageUrl = imageUrl,
                Message = "Image uploaded successfully"
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid image upload request");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to upload image");
            return StatusCode(500, new { message = "Failed to upload image to cloud storage" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during image upload");
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    /// Upload an image as Base64 string (application/json)
    [HttpPost("upload-base64")]
    [Consumes("application/json")]
    public async Task<ActionResult<ImageUploadResponseDto>> UploadImageBase64([FromBody] ImageUploadBase64RequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                return BadRequest(new { message = "Image data is required" });
            }

            var imageUrl = await _imageUploadService.UploadImageAsync(request.ImageBase64, request.ImageName);

            return Ok(new ImageUploadResponseDto
            {
                ImageUrl = imageUrl,
                Message = "Image uploaded successfully"
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid Base64 image upload request");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to upload Base64 image");
            return StatusCode(500, new { message = "Failed to upload image to cloud storage" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during Base64 image upload");
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    // imageFiles: List of image files to upload
    // returns: a list of ImageUploadResponseDto with ImgBB URLs
    [HttpPost("upload-multiple")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<List<ImageUploadResponseDto>>> UploadMultipleImages([FromForm] List<IFormFile> imageFiles)
    {
        try
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return BadRequest(new { message = "No image files provided" });
            }

            if (imageFiles.Count > 10)
            {
                return BadRequest(new { message = "Maximum 10 images allowed per request" });
            }

            var results = new List<ImageUploadResponseDto>();
            var errors = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                try
                {
                    if (_imageUploadService.ValidateImageFile(imageFile))
                    {
                        var imageUrl = await _imageUploadService.UploadImageAsync(imageFile);
                        results.Add(new ImageUploadResponseDto
                        {
                            ImageUrl = imageUrl,
                            Message = $"Image '{imageFile.FileName}' uploaded successfully"
                        });
                    }
                    else
                    {
                        errors.Add($"Invalid image file: {imageFile.FileName}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to upload image: {FileName}", imageFile.FileName);
                    errors.Add($"Failed to upload: {imageFile.FileName}");
                }
            }

            if (results.Count == 0)
            {
                return BadRequest(new { message = "No images were uploaded successfully", errors });
            }

            return Ok(new
            {
                uploadedImages = results,
                errors = errors.Count > 0 ? errors : null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during multiple image upload");
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }
    }

    /// Validate an image file without uploading
    [HttpPost("validate")]
    [Consumes("multipart/form-data")]
    public ActionResult<object> ValidateImage([FromForm] IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return BadRequest(new { valid = false, message = "No image file provided" });
        }

        var isValid = _imageUploadService.ValidateImageFile(imageFile);

        if (isValid)
        {
            return Ok(new
            {
                valid = true,
                message = "Image file is valid",
                fileName = imageFile.FileName,
                fileSize = imageFile.Length,
                contentType = imageFile.ContentType
            });
        }

        return BadRequest(new
        {
            valid = false,
            message = "Invalid image file. Allowed formats: jpg, jpeg, png, gif, bmp, webp. Maximum size: 32MB"
        });
    }
}
