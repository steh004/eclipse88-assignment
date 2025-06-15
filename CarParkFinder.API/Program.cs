using CarParkFinder.API.Data;
using CarParkFinder.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning); // Suppress verbose SQL

// Add services to the container
builder.Services.AddControllers();

// Add EF Core with SQLite/MySQL/PostgreSQL — here’s an SQLite example:
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<ICarParkService, CarParkService>();
builder.Services.AddScoped<CarParkCsvImporter>();
builder.Services.AddScoped<CarParkAvailabilityUpdater>();
builder.Services.AddHttpClient();

// Add Swagger for testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var importer = scope.ServiceProvider.GetRequiredService<CarParkCsvImporter>();
        await importer.ImportAsync();

        var updater = scope.ServiceProvider.GetRequiredService<CarParkAvailabilityUpdater>();
        await updater.UpdateAvailabilityAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Startup data tasks failed: {ex.Message}");
    }
}


app.Run();
