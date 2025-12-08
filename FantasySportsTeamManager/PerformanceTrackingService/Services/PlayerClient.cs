
using System.Net.Http.Json;
using PerformanceTrackingService.Models;

namespace PerformanceTrackingService.Services;

public class PlayerClient
{
    private readonly HttpClient _http;
    public PlayerClient(HttpClient http) => _http = http;

    public async Task<List<PlayerDto>> GetAllAsync(CancellationToken ct = default)
    {
        // Assumes Player service exposes GET /api/players returning JSON array
        var players = await _http.GetFromJsonAsync<List<PlayerDto>>("/api/players", cancellationToken: ct);
        return players ?? new List<PlayerDto>();
    }
}
