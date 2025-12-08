using Microsoft.EntityFrameworkCore;
using TeamManagement.Model;
namespace TeamManagement.Data
{
    public class TeamManagementContext : DbContext
    {
        public TeamManagementContext(DbContextOptions<TeamManagementContext> options) :base(options)
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
