
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PerformanceTrackingService.Controllers;
using PerformanceTrackingService.Services;
using Xunit;

namespace PerformanceTrackingService.Tests
{
    /// <summary>
    /// Smoke test for PerformanceController.Simulate without EF Core.
    /// It verifies the return shape when PlayerClient yields no drafted players.
    /// NOTE: If your Simulate() always writes to the DbContext, this test will fail—
    /// in that case keep only StatsGeneratorTests active until EF InMemory is installed.
    /// </summary>
    public class ControllersTests_NoEF
    {
        private class FakePlayerClient : PlayerClient
        {
            public FakePlayerClient() : base(new System.Net.Http.HttpClient()) { }
            public new Task<System.Collections.Generic.List<PerformanceTrackingService.Models.PlayerDto>> GetAllAsync(System.Threading.CancellationToken ct = default)
            {
                return Task.FromResult(new System.Collections.Generic.List<PerformanceTrackingService.Models.PlayerDto>());
            }
        }

        [Fact]
        public async Task Simulate_Returns_Zero_When_No_Players()
        {
            // Arrange
            var cfg = new ConfigurationBuilder().AddInMemoryCollection().Build();
            var controller = new PerformanceController(db: null!, playerClient: new FakePlayerClient(), gen: new StatsGenerator(1), cfg: cfg);

            // Act
            var res = await controller.Simulate(new PerformanceController.SimRequest("Week 1"), default);
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            var summary = Assert.IsType<PerformanceController.SimResult>(ok.Value);

            // Assert
            Assert.Equal("Week 1", summary.competition);
            Assert.Equal(0, summary.createdRecords);
        }
    }
}
