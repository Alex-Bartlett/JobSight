using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public interface ITaskRepository
    {
        /// <summary>
        /// Get a single task by id
        /// </summary>
        /// <param name="id">Task id</param>
        /// <returns>Task with matching id, or null if not found</returns>
        Task<JobTask?> GetByIdAsync(int id);

        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns>An IEnumerable of tasks for the job</returns>
        Task<IEnumerable<JobTask>> GetAllAsync(int jobId);
        Task<JobTask?> AddAsync(JobTask task);
        Task<JobTask?> UpdateAsync(JobTask updatedTask);
        Task DeleteAsync(int id);

    }
}
