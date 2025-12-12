using System.Net.Http.Json;
using PerformanceTrackingService.Models;

namespace PerformanceTrackingService.Services;

public class PlayerClient
{
    private readonly HttpClient _http;
    public PlayerClient(HttpClient http) => _http = http;

    public async Task<List<PlayerDto>> GetAllAsync(CancellationToken ct = default)
    {
        var response = await _http.GetFromJsonAsync<PlayerResponse>("/api/player", cancellationToken: ct);

        return response?.data ?? new List<PlayerDto>();
    }
}

public class PlayerResponse
{
    public string? message { get; set; }
    public List<PlayerDto>? data { get; set; }
}