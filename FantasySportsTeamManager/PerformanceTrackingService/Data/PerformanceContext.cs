
using Microsoft.EntityFrameworkCore;
using PerformanceTrackingService.Models;

namespace PerformanceTrackingService.Data;

public class PerformanceContext : DbContext
{
    public PerformanceContext(DbContextOptions<PerformanceContext> options) : base(options) { }
    public DbSet<PerformanceStat> PerformanceStats => Set<PerformanceStat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerformanceStat>(e =>
        {
            e.HasKey(x => x.statId);
            e.HasIndex(x => new { x.playerId, x.gameDate });
            e.Property(x => x.competitionName).HasMaxLength(100);
        });
    }
}
