
namespace PerformanceTrackingService.Services;

public class StatsGenerator
{
    private readonly Random _rng;

    public StatsGenerator() : this(Environment.TickCount) {}
    public StatsGenerator(int seed) => _rng = new Random(seed);

    public (int points, int assists, int rebounds) GenerateFor(string? position)
    {
        // Simple position-based ranges (tweak as you like)
        int p = position?.ToLowerInvariant() switch
        {
            "pg" or "g" => NextRange(8, 28),
            "sf" or "f" => NextRange(10, 30),
            "pf"        => NextRange(10, 26),
            "c"         => NextRange(8, 24),
            _           => NextRange(8, 26),
        };
        int a = NextRange(0, 10);
        int r = NextRange(2, 14);
        return (p, a, r);
    }

    private int NextRange(int min, int maxInclusive) => _rng.Next(min, maxInclusive + 1);
}
