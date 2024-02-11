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

        // DbSets are virtual for mock implementations: https://learn.microsoft.com/en-gb/ef/ef6/fundamentals/testing/mocking?redirectedfrom=MSDN#virtual-dbset-properties-with-ef-designer
        public virtual DbSet<AccountTier> AccountTiers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobNote> JobNotes { get; set; }
        public virtual DbSet<JobTask> JobTasks { get; set; }
        public virtual DbSet<JobTaskImage> JobTaskImages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

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
