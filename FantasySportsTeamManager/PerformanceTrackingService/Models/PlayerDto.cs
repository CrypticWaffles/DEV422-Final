
namespace PerformanceTrackingService.Models;

// Shape of Player service response (simplified)
public class PlayerDto
{
    public Guid playerId { get; set; }
    public string playerName { get; set; } = "";
    public string? position { get; set; }
    public Guid? teamId { get; set; }    // drafted if not null
}
