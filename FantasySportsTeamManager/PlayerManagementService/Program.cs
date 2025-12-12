
using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Get the connection string (Make sure "DefaultConnection" matches your appsettings.json)
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Register the Database Context
builder.Services.AddDbContext<FantasySportsContext>(options =>
    options.UseSqlServer(conn, sqlOptions =>
    {
        // This makes the app resilient to transient Azure errors
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    })
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Apply EF Core migrations and seed after app build
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<FantasySportsContext>();

        // Do NOT use EnsureCreated with migrations
        //  Use Migrate so EF applies your migration history
        context.Database.Migrate();

        // Optional: seed only if needed
        // e.g., if there are no players, seed the initial pool
        if (!context.Players.Any())
        {
            DataSeeder.SeedPlayers(context);
            logger.LogInformation("PlayerManagementService: Seeded initial players.");
        }
        else
        {
            logger.LogInformation("PlayerManagementService: Players already exist; skipping seed.");
        }
    }
    catch (Exception ex)
    {
        var logger2 = services.GetRequiredService<ILogger<Program>>();
        logger2.LogError(ex, "An error occurred while migrating/seeding the PlayerManagementService database.");
        throw; // Fail fast if migrations can’t run
    }
}



    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();