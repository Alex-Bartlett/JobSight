using JobSightLib.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
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
    }
}
