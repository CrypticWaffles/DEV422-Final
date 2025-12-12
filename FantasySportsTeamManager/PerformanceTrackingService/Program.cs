using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Services;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection missing");

builder.Services.AddDbContext<PerformanceContext>(options =>
    options.UseSqlServer(conn));

builder.Services.AddHttpClient<PlayerClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7063"); 
});
builder.Services.AddSingleton<StatsGenerator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"[Startup] DefaultConnection = {conn}");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Urls.Add("https://localhost:7284");
app.Urls.Add("http://localhost:5054");

app.Run();