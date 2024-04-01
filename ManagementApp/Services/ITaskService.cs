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
        Task<IEnumerable<JobTask>> GetAllAsync(int jobId);
        Task<JobTask?> GetByIdAsync(int id, User user);
        Task<JobTask?> AddAsync(JobTask task);
        Task<JobTask?> UpdateAsync(JobTask task);
        Task DeleteAsync(int id);
    }
}
