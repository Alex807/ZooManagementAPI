using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.Authentication;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birth date is required")]
    public DateOnly BirthDate { get; set; }

    public Gender? Gender { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")] 
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain exactly 10 digits")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Home address cannot exceed 255 characters")] 
    public string HomeAddress { get; set; } = string.Empty;

    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(2048, ErrorMessage = "Image URL cannot exceed 2048 characters")]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character(@ $ ! % * ? &)")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // defaults to Visitor if not provided
    public int? RoleId { get; set; } 
}