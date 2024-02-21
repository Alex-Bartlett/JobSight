using Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace Infrastructure
{
    public class JobSightDbContext : IdentityDbContext<User>

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

        /* When you make changes, use either:
        
           NuGet package manager console:
              Add-Migration [name]
              Update-Database
           .NET CLI (ctrl+'):
              dotnet ef migrations add [name]
              dotnet ef database update
        */

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
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
