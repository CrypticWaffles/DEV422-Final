
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PerformanceTrackingService.Controllers;
using PerformanceTrackingService.Data;
using PerformanceTrackingService.Models;
using PerformanceTrackingService.Services;
using Xunit;

public class ControllersTests
{
    private PerformanceContext MakeDb()
    {
        var opts = new DbContextOptionsBuilder<PerformanceContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new PerformanceContext(opts);
    }

    [Fact]
    public async Task GetAll_Returns_List()
    {
        using var db = MakeDb();
        db.PerformanceStats.Add(new PerformanceStat { playerId = Guid.NewGuid(), points = 10, assists = 3, rebounds = 5 });
        db.SaveChanges();
        var cfg = new ConfigurationBuilder().AddInMemoryCollection().Build();

        // Fake PlayerClient not used in GetAll
        var ctrl = new PerformanceController(db, new PlayerClient(new HttpClient()), new StatsGenerator(1), cfg);

        var result = await ctrl.GetAll(default);
        var ok = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        var list = Assert.IsAssignableFrom<IEnumerable<PerformanceStat>>(ok.Value);
        Assert.Single(list);
    }
}
