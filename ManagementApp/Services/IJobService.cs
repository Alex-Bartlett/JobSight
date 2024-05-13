using Shared.Models;

namespace ManagementApp.Services
{
    public interface IJobService : ICompanySpecificService
    {
        Task<IEnumerable<Job>> GetAllAsync(int companyId);

        /// <summary>
        /// Gets a single job
        /// </summary>
        /// <param name="jobId">The id of the job to get</param>
        /// <param name="user">The current user</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the job is not for the current company</exception>
        Task<Job?> GetByIdAsync(int jobId, User user);

        /// <summary>
        /// Creates a new job based on the given job object, and returns it.
        /// </summary>
        /// <param name="job">The job to create (id is irrelevant)</param>
        /// <returns>The new job from the database</returns>
        Task<Job?> CreateAsync(Job job);
        /// <summary>
        /// Updates a job in the database based on the given job object. All diffs will be uploaded for the job with the matching ID.
        /// </summary>
        /// <param name="job">The updated job</param>
        /// <returns>The updated job from the database</returns>
        Task<Job?> UpdateAsync(Job job);
        /// <summary>
        /// Deletes a job with a matching id and all associated tasks and notes.
        /// </summary>
        /// <param name="jobId">The id of the job to delete</param>
        Task DeleteAsync(int jobId);
    }
}
