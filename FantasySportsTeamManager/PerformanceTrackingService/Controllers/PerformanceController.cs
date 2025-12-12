
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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PerformanceStat>>> GetAll(CancellationToken ct)
    {
        var q = _db.PerformanceStats.AsNoTracking().OrderByDescending(s => s.gameDate);
        return Ok(await q.ToListAsync(ct));
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

        // 1) Get players and filter drafted
        var players = await _playerClient.GetAllAsync(ct);
        var drafted = players.Where(p => p.teamId is not null).ToList();

        // 2) Generate stats and persist
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

        // 3) Best-effort notify Leaderboard service (if configured)
        if (!string.IsNullOrWhiteSpace(_leaderboardBase))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var http = new HttpClient { BaseAddress = new Uri(_leaderboardBase!) };
                    await http.PostAsync("/api/leaderboard/refresh", content: null);
                }
                catch
                {
                    // ignore notification errors
                }
            });
        }

        return Ok(new SimResult(name!, rows.Count));
    }
}
