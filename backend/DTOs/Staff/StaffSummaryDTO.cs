namespace backend.DTOs.Staff;

// staff summary information (used in nested objects)
public class StaffSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Position { get; set; }
}