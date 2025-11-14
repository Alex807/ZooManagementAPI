using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);  //to serialize enums as strings in JSON responses in ZooManagementDbContext

// Add DbContext with MySQL
builder.Services.AddDbContext<ZooManagementDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))  //takes care of mySQL server version automatically
    )
);

var app = builder.Build();

app.UseDeveloperExceptionPage(); //detailed error pages for development

// Automatically create or update the database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ZooManagementDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization(); //used for future authentication/authorization

app.MapControllers();

app.Run();
