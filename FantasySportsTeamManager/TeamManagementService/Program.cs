using TeamManagementService.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<TeamManagementServiceContext>(options => 
{
    options.UseSqlServer(conn,
        sqlOptions =>
        { 
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount:5, 
                maxRetryDelay: TimeSpan.FromSeconds(30), 
                errorNumbersToAdd: null
            );
        }
    );
   
}
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TeamManagementServiceContext>();
    db.Database.EnsureCreated();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/*
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
*/

app.UseAuthorization();

/*
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});
*/
app.MapControllers();

app.Run();
