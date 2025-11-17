using backend.DTOs.Animal;

namespace backend.DTOs.Enclosure;

public class EnclosureResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public int? Capacity { get; set; }
    public string? Location { get; set; }
    public int CurrentOccupancy { get; set; }
    public bool IsAtCapacity { get; set; }
    public List<AnimalSummaryDto> Animals { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}