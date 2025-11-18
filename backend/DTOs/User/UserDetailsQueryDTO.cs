using backend.Enums;

namespace backend.DTOs.User;

public class UserDetailsQueryDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public DateOnly? BirthDateFrom { get; set; }
    public DateOnly? BirthDateTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "UserId";
    public bool SortDescending { get; set; } = false;
}