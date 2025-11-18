namespace backend.DTOs.Category;

public class CategoryQueryDto
{
    public string? Name { get; set; }
    public int? MinAnimalCount { get; set; }
    public int? MaxAnimalCount { get; set; }
}