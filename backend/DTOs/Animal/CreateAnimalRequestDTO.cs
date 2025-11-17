using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.Animal;

public class CreateAnimalRequestDto
{
    [Required(ErrorMessage = "Animal name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Species is required")]
    [StringLength(50, ErrorMessage = "Species cannot exceed 50 characters")]
    public string Specie { get; set; } = string.Empty;

    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(2048, ErrorMessage = "Image URL cannot exceed 2048 characters")]
    public string ImageUrl { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public Gender? Gender { get; set; }

    public DateTime? ArrivalDate { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    public int CategoryId { get; set; }

    public int? EnclosureId { get; set; }
}