
using Microsoft.EntityFrameworkCore;
using FantasySportsTeamManager.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Services
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection missing");

builder.Services.AddDbContext<LeaderboardContext>(options =>
    options.UseSqlServer(conn));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: log the effective connection string (dev-only)
Console.WriteLine($"[Startup] DefaultConnection = {conn}");

var app = builder.Build();

// 2) Startup migration guard (prevents duplicate create errors)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LeaderboardContext>();
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

// 4) Bind Kestrel to known ports (match launchSettings.json)
app.Urls.Add("https://localhost:7284");
app.Urls.Add("http://localhost:5054");

app.Run();
