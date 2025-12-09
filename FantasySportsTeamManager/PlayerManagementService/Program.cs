using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FantasySportsDb");

builder.Services.AddDbContext<FantasySportsContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FantasySportsDb"),
        sqlOptions =>
        {
            // Enable retry on failure for transient faults
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Try 5 times
                maxRetryDelay: TimeSpan.FromSeconds(30), // Wait up to 30 seconds between tries
                errorNumbersToAdd: null
            );
        }
    ));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Get the database context
        var context = services.GetRequiredService<FantasySportsContext>();

        // Ensure the database is created
        context.Database.EnsureCreated();

        // Run the seeder
        DataSeeder.SeedPlayers(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
