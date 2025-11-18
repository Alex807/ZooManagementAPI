namespace backend.DTOs.Enclosure;

public class EnclosureQueryDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Location { get; set; }
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
    public bool? AtCapacity { get; set; }
}