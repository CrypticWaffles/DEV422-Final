using Microsoft.EntityFrameworkCore;
using TeamManagementService.Data;
using TeamManagementService.Model;
namespace TeamManagementService.Data
{
    public class TeamManagementServiceContext : DbContext
    {
        public TeamManagementServiceContext(DbContextOptions<TeamManagementServiceContext> options) :base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<Team>().HasData(
                new Team {
                    teamId = -1,
                    teamName = "string",
                    createdDate = DateTime.Now
                }
            );
            modelBuilder.Entity<Team>()
                .Property(t => t.teamId)
                .HasColumnName("teamId");
        }
    }
}
