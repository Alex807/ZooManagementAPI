using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Text.Json.Serialization; 
using Scalar.AspNetCore;  

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);  //to serialize enums as strings in JSON responses in ZooManagementDbContext

// Add Swagger/OpenAPI generation for Scalar
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with MySQL
builder.Services.AddDbContext<ZooManagementDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))  //takes care of mySQL server version automatically
    )
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //print detailed error (stack trace) for development
}

// Automatically create or update the database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ZooManagementDbContext>();
    dbContext.Database.Migrate();
}


app.UseHttpsRedirection();

app.UseCors("AllowAll");  //in order to use CORS policy (comes with builder.Services.AddCors)

app.UseAuthorization(); //used for future authentication/authorization

// json endpoint (../swagger/v1/swagger.json)
app.UseSwagger(); //needed for Swagger JSON endpoint that is readed by Scalar UI

// Scalar UI documentation
app.MapScalarApiReference(options =>
{
    options.Theme = ScalarTheme.Mars; 
    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");  //needed to point to the correct swagger json endpoint
});

app.MapControllers();

app.Run(); //CTRL + F5(for ScalarUI pop-up)
