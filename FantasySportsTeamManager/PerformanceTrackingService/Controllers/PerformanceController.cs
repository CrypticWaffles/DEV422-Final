
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Models;
using PerformanceTrackingService.Services;

namespace PerformanceTrackingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerformanceController : ControllerBase
{
    private readonly PerformanceContext _db;
    private readonly PlayerClient _playerClient;
    private readonly StatsGenerator _gen;
    private readonly string? _leaderboardBase;

    public PerformanceController(
        PerformanceContext db,
        PlayerClient playerClient,
        StatsGenerator gen,
        IConfiguration cfg)
    {
        _db = db;
        _playerClient = playerClient;
        _gen = gen;
        _leaderboardBase = cfg["LeaderboardServiceBaseUrl"]?.TrimEnd('/');
    }

    // GET /api/performance
    // If no stats exist yet but there are drafted players, auto-seed one batch
    // so the list is not empty during demos/tests.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PerformanceStat>>> GetAll(CancellationToken ct)
    {
        var existing = await _db.PerformanceStats
            .AsNoTracking()
            .OrderByDescending(s => s.gameDate)
            .ToListAsync(ct);

        if (existing.Count > 0)
            return Ok(existing);

        // No stats yet → best-effort auto-seed for drafted players
        var players = await _playerClient.GetAllAsync(ct);
        var drafted = players.Where(p => p.teamId is not null).ToList();

        if (drafted.Count == 0)
            return Ok(existing); // still empty, nothing to seed

        var now = DateTimeOffset.UtcNow;
        var rows = drafted.Select(p =>
        {
            var (pts, ast, reb) = _gen.GenerateFor(p.position);
            return new PerformanceStat
            {
                playerId = p.playerId,
                points = pts,
                assists = ast,
                rebounds = reb,
                gameDate = now,
                competitionName = "AutoSeed"
            };
        }).ToList();

        _db.PerformanceStats.AddRange(rows);
        await _db.SaveChangesAsync(ct);

        // Return freshly seeded rows
        var seeded = await _db.PerformanceStats
            .AsNoTracking()
            .OrderByDescending(s => s.gameDate)
            .ToListAsync(ct);

        return Ok(seeded);
    }

    // GET /api/performance/{playerId}
    [HttpGet("{playerId:guid}")]
    public async Task<ActionResult<IEnumerable<PerformanceStat>>> GetByPlayer(Guid playerId, CancellationToken ct)
    {
        var q = _db.PerformanceStats.AsNoTracking()
            .Where(s => s.playerId == playerId)
            .OrderByDescending(s => s.gameDate);
        return Ok(await q.ToListAsync(ct));
    }

    // POST /api/performance/simulate
    public record SimRequest(string competitionName);
    public record SimResult(string competition, int createdRecords);

    [HttpPost("simulate")]
    public async Task<ActionResult<SimResult>> Simulate([FromBody] SimRequest req, CancellationToken ct)
    {
        var name = req.competitionName?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("competitionName is required");

        var players = await _playerClient.GetAllAsync(ct);
        var drafted = players.Where(p => p.teamId is not null).ToList();

        var now = DateTimeOffset.UtcNow;
        var rows = drafted.Select(p =>
        {
            var (pts, ast, reb) = _gen.GenerateFor(p.position);
            return new PerformanceStat
            {
                playerId = p.playerId,
                points = pts,
                assists = ast,
                rebounds = reb,
                gameDate = now,
                competitionName = name
            };
        }).ToList();

        _db.PerformanceStats.AddRange(rows);
        await _db.SaveChangesAsync(ct);

        if (!string.IsNullOrWhiteSpace(_leaderboardBase))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var http = new HttpClient { BaseAddress = new Uri(_leaderboardBase!) };
                    await http.PostAsync("/api/leaderboard/refresh", content: null);
                }
                catch { /* ignore */ }
            });
        }

        return Ok(new SimResult(name!, rows.Count));
    }
}
