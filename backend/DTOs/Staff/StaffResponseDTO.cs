namespace backend.DTOs.Staff;

public class StaffResponseDto
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public decimal? Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}