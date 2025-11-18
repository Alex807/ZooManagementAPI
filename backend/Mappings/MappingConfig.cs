using Mapster;
using backend.DTOs.Animal;
using backend.DTOs.Assignment;
using backend.DTOs.Category;
using backend.DTOs.Enclosure;
using backend.DTOs.FeedingSchedule;
using backend.DTOs.MedicalRecord;
using backend.DTOs.Staff;
using backend.DTOs.User;
using backend.Models;

namespace backend.Mappings;

// Mapster mapping configuration for all DTOs
public static class MappingConfig
{
    public static void Configure()
    {
        // ===========================
        // Animal Mappings
        // ===========================
        TypeAdapterConfig<Animal, AnimalResponseDto>
            .NewConfig()
            .Map(dest => dest.Age, src => src.DateOfBirth.HasValue
                ? DateTime.Now.Year - src.DateOfBirth.Value.Year
                : (int?)null);

        TypeAdapterConfig<Animal, AnimalSummaryDto>.NewConfig();

        // ===========================
        // Category Mappings
        // ===========================
        TypeAdapterConfig<Category, CategoryResponseDto>
            .NewConfig()
            .Map(dest => dest.AnimalCount, src => src.Animals != null ? src.Animals.Count : 0);

        TypeAdapterConfig<Category, CategorySummaryDto>.NewConfig();

        // ===========================
        // Enclosure Mappings
        // ===========================
        TypeAdapterConfig<Enclosure, EnclosureResponseDto>
            .NewConfig()
            .Map(dest => dest.CurrentOccupancy, src => src.Animals != null ? src.Animals.Count : 0)
            .Map(dest => dest.IsAtCapacity, src => src.Animals != null
                ? src.Animals.Count >= src.Capacity
                : false);

        TypeAdapterConfig<Enclosure, EnclosureSummaryDto>.NewConfig();

        // ===========================
        // User Mappings
        // ===========================
        TypeAdapterConfig<UserAccount, UserResponseDto>
            .NewConfig()
            .Map(dest => dest.CurrentRole, src => src.CurrentRole != null ? src.CurrentRole.Name.ToString() : string.Empty)
            .Map(dest => dest.Roles, src => src.UserRoles != null
                ? src.UserRoles.Select(ur => ur.Role.Name.ToString()).ToList()
                : new List<string>());

        TypeAdapterConfig<UserDetails, UserDetailsDto>.NewConfig();

        // ===========================
        // Staff Mappings
        // ===========================
        TypeAdapterConfig<Staff, StaffResponseDto>
            .NewConfig()
            .Map(dest => dest.Username, src => src.UserAccount != null ? src.UserAccount.Username : string.Empty)
            .Map(dest => dest.FullName, src => src.UserAccount != null && src.UserAccount.UserDetails != null
                ? $"{src.UserAccount.UserDetails.FirstName} {src.UserAccount.UserDetails.LastName}".Trim()
                : string.Empty);

        TypeAdapterConfig<Staff, StaffSummaryDto>
            .NewConfig()
            .Map(dest => dest.FullName, src => src.UserAccount != null && src.UserAccount.UserDetails != null
                ? $"{src.UserAccount.UserDetails.FirstName} {src.UserAccount.UserDetails.LastName}".Trim()
                : string.Empty);

        // ===========================
        // FeedingSchedule Mappings
        // ===========================
        TypeAdapterConfig<FeedingSchedule, FeedingScheduleResponseDto>.NewConfig();

        // ===========================
        // MedicalRecord Mappings
        // ===========================
        TypeAdapterConfig<MedicalRecord, MedicalRecordResponseDto>
            .NewConfig()
            .Map(dest => dest.Veterinarian, src => src.Staff);

        // ===========================
        // StaffAnimalAssignment Mappings
        // ===========================
        TypeAdapterConfig<StaffAnimalAssignment, StaffAnimalAssignmentResponseDto>
            .NewConfig()
            .Map(dest => dest.StaffName, src => src.Staff != null && src.Staff.UserAccount != null && src.Staff.UserAccount.UserDetails != null
                ? $"{src.Staff.UserAccount.UserDetails.FirstName} {src.Staff.UserAccount.UserDetails.LastName}".Trim()
                : string.Empty)
            .Map(dest => dest.AnimalName, src => src.Animal != null ? src.Animal.Name : string.Empty);

        // ===========================
        // Global Settings
        // ===========================
        TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
    }
}