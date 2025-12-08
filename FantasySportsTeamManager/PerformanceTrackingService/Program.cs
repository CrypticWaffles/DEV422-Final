using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// EF Core: SQL Server
var conn = builder.Configuration.GetConnectionString("PerformanceDb")
           ?? throw new InvalidOperationException("ConnectionStrings:PerformanceDb missing");
builder.Services.AddDbContext<PerformanceContext>(opt => opt.UseSqlServer(conn));

// Register the retry policy as a singleton so it can be injected into the DelegatingHandler
builder.Services.AddSingleton<IAsyncPolicy<HttpResponseMessage>>(GetRetryPolicy());

// Register the policy delegating handler
builder.Services.AddTransient<PolicyHttpMessageHandler>();

// HTTP Client for Player service using the DelegatingHandler that applies the Polly policy
builder.Services.AddHttpClient<PlayerClient>(client =>
{
    var baseUrl = builder.Configuration["PlayerServiceBaseUrl"] ?? "";
    if (!string.IsNullOrWhiteSpace(baseUrl)) client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<PolicyHttpMessageHandler>();

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

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .OrResult(msg => !msg.IsSuccessStatusCode)
        .WaitAndRetryAsync(new[]
        {
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1)
        });
}

// DelegatingHandler that applies the injected Polly policy to outbound HTTP calls
internal class PolicyHttpMessageHandler : DelegatingHandler
{
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public PolicyHttpMessageHandler(IAsyncPolicy<HttpResponseMessage> policy)
    {
        _policy = policy ?? throw new ArgumentNullException(nameof(policy));
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Execute the outgoing request through the policy
        return _policy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
    }
}
