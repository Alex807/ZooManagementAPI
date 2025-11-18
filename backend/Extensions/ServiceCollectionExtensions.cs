using backend.Services.Animals;
using backend.Services.Categories;
using backend.Services.Enclosures;
using backend.Services.Staff;
using backend.Services.MedicalRecords;
using backend.Services.FeedingSchedules;

namespace backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IEnclosureService, EnclosureService>(); 
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IFeedingScheduleService, FeedingScheduleService>();
        // add more here...

        return services;
    }
}
