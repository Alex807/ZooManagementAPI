using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.User;

public class UpdateUserDetailsRequestDto
{
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string? FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Gender? Gender { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")] 
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain exactly 10 digits")]
    public string? Phone { get; set; }

    [StringLength(255, ErrorMessage = "Home address cannot exceed 255 characters")]
    public string? HomeAddress { get; set; } 

    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(2048, ErrorMessage = "Image URL cannot exceed 2048 characters")]
    public string? ImageUrl { get; set; }
}