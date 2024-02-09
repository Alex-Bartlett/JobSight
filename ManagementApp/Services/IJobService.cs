using Shared.Models;

namespace ManagementApp.Services
{
    public interface IJobService : ICompanySpecificService
    {
        Task<IEnumerable<Job>> GetAllJobs();
        Task<Job> GetJob(int jobId);
    }
}
