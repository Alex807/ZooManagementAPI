using backend.Services.Animals;
using backend.Services.Categories;
using backend.Services.Enclosures;
using backend.Services.Staff;

namespace backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IEnclosureService, EnclosureService>(); 
        services.AddScoped<IStaffService, StaffService>();
        // add more here...

        return services;
    }
}
