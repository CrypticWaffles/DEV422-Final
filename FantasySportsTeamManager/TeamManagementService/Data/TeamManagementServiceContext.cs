using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<Team>().HasData(
                new Team{
                    teamId=0,
                    teamName="string",
                    createdDate=DateTime.Now
                }
            );
        }
    }
}
