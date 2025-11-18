namespace backend.DTOs.Staff;

public class StaffQueryDto
{
    public string? Department { get; set; }
    public string? Position { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public DateTime? HiredAfter { get; set; }
    public DateTime? HiredBefore { get; set; }
}