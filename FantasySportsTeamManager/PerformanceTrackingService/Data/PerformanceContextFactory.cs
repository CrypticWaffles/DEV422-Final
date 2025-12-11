
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PerformanceTrackingService.Data
{
    public class PerformanceContextFactory : IDesignTimeDbContextFactory<PerformanceContext>
    {
        public PerformanceContext CreateDbContext(string[] args)
        {
            // Load appsettings.json from the project’s content root
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PerformanceContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PerformanceContext(optionsBuilder.Options);
        }
    }
}
