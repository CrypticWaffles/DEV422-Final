
using Microsoft.EntityFrameworkCore;
using FantasySportsTeamManager.Data;

var builder = WebApplication.CreateBuilder(args);

// DB connection
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection missing");

builder.Services.AddDbContext<LeaderboardContext>(options =>
    options.UseSqlServer(conn, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                        maxRetryDelay: TimeSpan.FromSeconds(30),
                                        errorNumbersToAdd: null);
    }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LeaderboardContext>();
    db.Database.Migrate();   // applies EF migrations if present
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
