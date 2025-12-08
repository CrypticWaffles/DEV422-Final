
using Xunit;
using PerformanceTrackingService.Services;

namespace PerformanceTrackingService.Tests
{
    public class StatsGeneratorTests
    {
        [Fact]
        public void Generates_Deterministic_With_Same_Seed()
        {
            var g1 = new StatsGenerator(123);
            var g2 = new StatsGenerator(123);

            var s1 = g1.GenerateFor("PG");
            var s2 = g2.GenerateFor("PG");

            Assert.Equal(s1, s2);
        }

        [Fact]
        public void Generates_Values_In_Expected_Ranges()
        {
            var gen = new StatsGenerator(1);
            var (p, a, r) = gen.GenerateFor("C");

            Assert.InRange(p, 8, 30);
            Assert.InRange(a, 0, 10);
            Assert.InRange(r, 2, 14);
        }
    }
}
