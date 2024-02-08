using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class JobSightDbContext : DbContext
    {
        public JobSightDbContext(DbContextOptions<JobSightDbContext> options) : base(options)
        {
        }

        public DbSet<AccountTier> AccountTiers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobNote> JobNotes { get; set; }
        public DbSet<JobTask> JobTasks { get; set; }
        public DbSet<JobTaskImage> JobTaskImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        /* When you make changes, use either:
        
           NuGet package manager console:
              Add-Migration [name]
              Update-Database
           .NET CLI (ctrl+'):
              dotnet ef migrations add [name]
              dotnet ef database update
        */
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<JobSightDbContext>
    {
        // https://medium.com/oppr/net-core-using-entity-framework-core-in-a-separate-project-e8636f9dc9e5

        public JobSightDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Infrastructure/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<JobSightDbContext>();
            var connectionString = configuration.GetConnectionString("Development");
            builder.UseNpgsql(connectionString);
            return new JobSightDbContext(builder.Options);
        }
    }
}
