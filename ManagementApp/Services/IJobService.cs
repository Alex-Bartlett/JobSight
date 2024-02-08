using Shared.Models;

namespace ManagementApp.Services
{
    public interface IJobService
    {
        Task<List<Job>> GetAllJobs();
        Task<Job> GetJob(int jobId);
    }
}
