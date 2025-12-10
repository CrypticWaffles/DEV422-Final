
using System;
using Microsoft.EntityFrameworkCore;

namespace FantasySportsTeamManager.Data
{
    public class LeaderboardContext : DbContext
    {
        public LeaderboardContext(DbContextOptions<LeaderboardContext> options) : base(options) { }

        public DbSet<LeaderboardRow> Leaderboard { get; set; } = null!;
    }

    public class LeaderboardRow
    {
        public Guid TeamId { get; set; }             // matches Teams.teamId (GUID)
        public int TotalPoints { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
}
