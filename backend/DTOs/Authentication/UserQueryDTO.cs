namespace backend.DTOs.Authentication;

public class UserQueryDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "Id";
    public bool SortDescending { get; set; } = false;
}