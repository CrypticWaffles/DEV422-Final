
using Microsoft.EntityFrameworkCore;

namespace FantasySportsTeamManager.Data
{
    public class LeaderboardContext : DbContext
    {
        public LeaderboardContext(DbContextOptions<LeaderboardContext> options) : base(options) { }

        public DbSet<LeaderboardRow> Leaderboard { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaderboardRow>(entity =>
            {
                entity.HasKey(e => e.TeamId);          // set PK
                entity.ToTable("Leaderboard");         // table name in DB
                entity.Property(e => e.TotalPoints).IsRequired();
                entity.Property(e => e.LastUpdatedUtc).IsRequired();
            });
        }
    }

    public class LeaderboardRow
    {
        public Guid TeamId { get; set; }             // PK
        public int TotalPoints { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
}
