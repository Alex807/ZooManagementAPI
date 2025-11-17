using backend.Enums;

namespace backend.DTOs.User;

public class UserDetailsDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}".Trim();
    public DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? HomeAddress { get; set; }
    public string? ImageUrl { get; set; }
}