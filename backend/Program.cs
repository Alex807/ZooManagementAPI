using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using backend.Mappings;
using backend.Extensions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System; //for the env variable where JWT key is stored

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(); //custom extension method to add all application services in one place

// Configure Mapster
MappingConfig.Configure();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //fails fast if the env var is missing
    var key = Environment.GetEnvironmentVariable("Jwt__Key")
              ?? throw new InvalidOperationException("Jwt__Key environment variable is not set.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Configure Authorization Policies
builder.Services.AddAuthorizationBuilder()
                                .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
                                .AddPolicy("ZookeeperOrAbove", policy => policy.RequireRole("Admin", "Zookeeper"))
                                .AddPolicy("VeterinarianOrAbove", policy => policy.RequireRole("Admin", "Veterinarian"))
                                .AddPolicy("StaffOrAbove", policy => policy.RequireRole("Admin", "Zookeeper", "Veterinarian"));
                                //already default role for authenticated users is Visitor

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

builder.Services.AddHealthChecks(); //to check if the database connection is healthy

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

app.UseStaticFiles(); //to serve the static files (Scalar UI files in wwwroot folder)

// json endpoint (../swagger/v1/swagger.json)
app.UseSwagger(); //needed for Swagger JSON endpoint that is readed by Scalar UI

app.UseCors("AllowAll");  //in order to use CORS policy (comes with builder.Services.AddCors)

app.UseAuthentication(); //used for future authentication/authorization

app.UseAuthorization(); //used for future authentication/authorization

// Scalar UI documentation
app.MapScalarApiReference(options =>
{
    options.Theme = ScalarTheme.Mars;
    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");  //needed to point to the correct swagger json endpoint
});

app.MapControllers();

app.MapHealthChecks("/health"); //see if the API is running

app.Run(); //CTRL + F5(for ScalarUI pop-up)
