
using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Services;

var builder = WebApplication.CreateBuilder(args);

// EF Core: SQL Server
var conn = builder.Configuration.GetConnectionString("PerformanceDb")
           ?? throw new InvalidOperationException("ConnectionStrings:PerformanceDb missing");
builder.Services.AddDbContext<PerformanceContext>(opt => opt.UseSqlServer(conn));

// HTTP Client for Player service
builder.Services.AddHttpClient<PlayerClient>(client =>
{
    var baseUrl = builder.Configuration["PlayerServiceBaseUrl"] ?? "";
    if (!string.IsNullOrWhiteSpace(baseUrl)) client.BaseAddress = new Uri(baseUrl);
}).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(new[]
{
    TimeSpan.FromMilliseconds(200),
    TimeSpan.FromMilliseconds(500),
    TimeSpan.FromSeconds(1)
}));

builder.Services.AddSingleton<StatsGenerator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger UI

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();
app.Run();
