using backend.DTOs.Category;
using backend.DTOs.Enclosure;
using backend.Enums;

namespace backend.DTOs.Animal;

// animal response with complete information
public class AnimalResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specie { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public int? Age { get; set; }
    public Gender? Gender { get; set; }
    public DateTime ArrivalDate { get; set; }
    public CategorySummaryDto Category { get; set; } = null!;
    public EnclosureSummaryDto? Enclosure { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}