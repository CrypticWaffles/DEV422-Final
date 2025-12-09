using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Model;

namespace PlayerManagementService.Data
{
    public class FantasySportsContext : DbContext
    {
        public FantasySportsContext(DbContextOptions<FantasySportsContext> options)
            : base(options)
        {
        }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Player>().ToTable("Players");

            modelBuilder.Entity<Player>()
                .Property(p => p.PlayerId)
                .HasColumnName("playerId");
        }
    }
}