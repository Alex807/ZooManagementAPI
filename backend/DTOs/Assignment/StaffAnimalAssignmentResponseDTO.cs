namespace backend.DTOs.Assignment;

public class StaffAnimalAssignmentResponseDto
{
    public int StaffId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public int AnimalId { get; set; }
    public string AnimalName { get; set; } = string.Empty;
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}