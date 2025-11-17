using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.Animal;

public class UpdateAnimalRequestDto
{
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string? Name { get; set; }

    [StringLength(50, ErrorMessage = "Species cannot exceed 50 characters")]
    public string? Specie { get; set; }

    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(2048, ErrorMessage = "Image URL cannot exceed 2048 characters")]
    public string? ImageUrl { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public int? CategoryId { get; set; }

    public int? EnclosureId { get; set; }
}