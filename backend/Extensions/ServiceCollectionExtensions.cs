using backend.Services.Animals;
using backend.Services.Categories;
using backend.Services.Enclosures;

namespace backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IEnclosureService, EnclosureService>();
        // add more here...

        return services;
    }
}
