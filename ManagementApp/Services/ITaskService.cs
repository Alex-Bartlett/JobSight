using Shared.Models;

namespace ManagementApp.Services
{
    public interface ITaskService
    {
        /// <summary>
        /// Gets all tasks for a job. No authorization checks are performed, as authorisation for the job is assumed. Do not use this method if the user's access to the job is not already validated.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<IEnumerable<JobTask>> GetAllAsync(int jobId, bool refreshImageUrls = true);
        Task<JobTask?> GetByIdAsync(int id, User user, bool refreshImageUrls = true);
        Task<JobTask?> CreateAsync(JobTask task);
        Task<JobTask?> UpdateAsync(JobTask task);
        Task DeleteAsync(int id, User user);
    }
}
