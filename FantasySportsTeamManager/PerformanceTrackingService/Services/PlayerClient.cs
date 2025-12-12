
using System.Net.Http.Json;
using System.Text.Json;
using PerformanceTrackingService.Models;

namespace PerformanceTrackingService.Services;

public class PlayerClient
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PlayerClient(HttpClient http) => _http = http;

    public async Task<List<PlayerDto>> GetAllAsync(CancellationToken ct = default)
    {
        // PlayerManagementService returns { Message, Data: [...] } at /api/player
        var env = await _http.GetFromJsonAsync<PlayerResponse>("/api/player", _json, ct);
        return env?.Data ?? new List<PlayerDto>();
    }
}

public class PlayerResponse
{
    public string? Message { get; set; }
    public List<PlayerDto>? Data { get; set; }
}
