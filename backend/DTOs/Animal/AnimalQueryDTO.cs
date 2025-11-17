using backend.Enums;

namespace backend.DTOs.Animal;

public class AnimalQueryDto
{
    public string? Name { get; set; }
    public string? Specie { get; set; }
    public Gender? Gender { get; set; }
    public int? CategoryId { get; set; }
    public int? EnclosureId { get; set; }
    public DateTime? ArrivalDateFrom { get; set; }
    public DateTime? ArrivalDateTo { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
}