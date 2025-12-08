
namespace PerformanceTrackingService.Models;

public class PerformanceStat
{
    public Guid statId { get; set; } = Guid.NewGuid();
    public Guid playerId { get; set; }
    public int points { get; set; }
    public int assists { get; set; }
    public int rebounds { get; set; }
    public DateTimeOffset gameDate { get; set; } = DateTimeOffset.UtcNow;
    public string? competitionName { get; set; }
}

