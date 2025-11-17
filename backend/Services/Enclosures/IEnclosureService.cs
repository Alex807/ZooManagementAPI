using backend.DTOs.Enclosure;

namespace backend.Services.Enclosures;

public interface IEnclosureService
{
    Task<IEnumerable<EnclosureResponseDto>> GetAllEnclosuresAsync(EnclosureQueryDto query);
    Task<EnclosureResponseDto?> GetEnclosureByIdAsync(int id);
    Task<EnclosureResponseDto> CreateEnclosureAsync(CreateEnclosureRequestDto request);
    Task<EnclosureResponseDto?> UpdateEnclosureAsync(int id, UpdateEnclosureRequestDto request);
    Task<bool> DeleteEnclosureAsync(int id);
    Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByNameAsync(string name);
    Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByTypeAsync(string type);
    Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByLocationAsync(string location);
    Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresByCapacityAsync(int? min, int? max);
    Task<IEnumerable<EnclosureResponseDto>> GetEnclosuresAtCapacityAsync();
    Task<IEnumerable<EnclosureResponseDto>> GetAvailableEnclosuresAsync();
    Task<IEnumerable<EnclosureResponseDto>> GetEmptyEnclosuresAsync();
}