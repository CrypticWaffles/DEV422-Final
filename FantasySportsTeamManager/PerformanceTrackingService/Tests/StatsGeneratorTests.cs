
using PerformanceTrackingService.Services;
using Xunit;

public class StatsGeneratorTests
{
    [Fact]
    public void Generates_Deterministic_With_Seed()
    {
        var g1 = new StatsGenerator(123);
        var g2 = new StatsGenerator(123);
        var s1 = g1.GenerateFor("PG");
        var s2 = g2.GenerateFor("PG");
        Assert.Equal(s1, s2);
    }
}
