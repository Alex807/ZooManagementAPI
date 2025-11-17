using backend.DTOs.Animal;
using backend.Enums;

namespace backend.Services.Animals;

public interface IAnimalService
{
    Task<IEnumerable<AnimalResponseDto>> GetAllAnimalsAsync(AnimalQueryDto query);
    Task<AnimalResponseDto?> GetAnimalByIdAsync(int id);
    Task<AnimalResponseDto> CreateAnimalAsync(CreateAnimalRequestDto request);
    Task<AnimalResponseDto?> UpdateAnimalAsync(int id, UpdateAnimalRequestDto request);
    Task<bool> DeleteAnimalAsync(int id);
    Task<IEnumerable<AnimalResponseDto>> GetAnimalsByCategoryAsync(int categoryId);
    Task<IEnumerable<AnimalResponseDto>> GetAnimalsByEnclosureAsync(int enclosureId);
    Task<IEnumerable<AnimalResponseDto>> GetAnimalsBySpecieAsync(string specie);
    Task<IEnumerable<AnimalResponseDto>> GetAnimalsByGenderAsync(Gender gender);
    Task<IEnumerable<AnimalResponseDto>> GetAnimalsByAgeRangeAsync(int? minAge, int? maxAge);
    Task<IEnumerable<AnimalResponseDto>> GetAnimalsByArrivalDateAsync(DateTime? from, DateTime? to);
}