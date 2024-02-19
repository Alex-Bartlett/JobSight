using Shared.Models;

namespace ManagementApp.Services
{
    public interface IJobService : ICompanySpecificService
    {
        Task<IEnumerable<Job>> GetAllAsync();
        Task<Job?> GetByIdAsync(int jobId);
    }
}
