using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public interface IJobRepository
    {
        /// <summary>
        /// Get a single job by id
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns>Job with matching id, or null if not found</returns>
        Task<Job?> GetByIdAsync(int id);

        /// <summary>
        /// Get all jobs
        /// </summary>
        /// <returns>An IEnumerable of jobs for company</returns>
        Task<IEnumerable<Job>> GetAllAsync(int companyId);
        Task<Job?> AddAsync(Job job);
        Task<Job?> UpdateAsync(Job job);
        Task DeleteAsync(int id);

    }
}
