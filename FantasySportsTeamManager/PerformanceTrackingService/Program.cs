using Microsoft.EntityFrameworkCore;
// FIX 1: Use the correct namespace for this project
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Services
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection missing");

// FIX 2: Use PerformanceContext instead of LeaderboardContext
builder.Services.AddDbContext<PerformanceContext>(options =>
    options.UseSqlServer(conn));

// FIX 3: Register the services required by your PerformanceController
// (Found in your code.txt lines 10 and 16)
builder.Services.AddHttpClient<PlayerClient>(client =>
{
    // You likely need a BaseAddress here for the Player service, e.g.:
    client.BaseAddress = new Uri("https://localhost:7063"); 
});
builder.Services.AddSingleton<StatsGenerator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: log the effective connection string (dev-only)
Console.WriteLine($"[Startup] DefaultConnection = {conn}");

var app = builder.Build();

// 2) Startup migration guard
using (var scope = app.Services.CreateScope())
{
    // FIX 4: Retrieve the correct context here as well
    var db = scope.ServiceProvider.GetRequiredService<PerformanceContext>();
    var pending = db.Database.GetPendingMigrations().ToList();

    if (pending.Any())
    {
        Console.WriteLine($"[Startup] Applying {pending.Count} migration(s): {string.Join(", ", pending)}");
        db.Database.Migrate();
        Console.WriteLine("[Startup] Migrations applied successfully.");
    }
    else
    {
        Console.WriteLine("[Startup] No pending migrations.");
    }
}

// 3) Middleware & endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 4) Bind Kestrel to known ports
app.Urls.Add("https://localhost:7284");
app.Urls.Add("http://localhost:5054");

app.Run();