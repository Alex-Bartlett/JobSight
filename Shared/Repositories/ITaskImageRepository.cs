using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public interface ITaskImageRepository
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="taskImageId">The ID of the task image</param>
        /// <param name="companyId">Optional: The company ID for the task. If not provided, a denser query is performed to get this information. If incorrect, image might not be able to be retrieved. If you have it to hand, supply it.</param>
        /// <returns>The task image object</returns>
        Task<JobTaskImage?> GetByIdAsync(string taskImageId, int? companyId = null);

        /// <summary>
        /// Gets all image tasks for a given task.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="companyId">Optional: The company ID for the task. If not provided, a denser query is performed to get this information. If incorrect, image might not be able to be retrieved. If you have it to hand, supply it.</param>
        /// <returns></returns>
        Task<IEnumerable<JobTaskImage>> GetAllAsync(int taskId, int? companyId = null);

        /// <summary>
        /// Adds a new task image and uploads the image file to the storage service.
        /// </summary>
        /// <param name="taskImage">The task image object to add</param>
        /// <param name="imageFile">The relevant image file in bytes to add</param>
        /// <returns>The new task image object</returns>
        Task<JobTaskImage?> AddAsync(JobTaskImage taskImage, byte[] imageFile);

        /// <summary>
        /// Deletes a task image from the database and the storage service.
        /// </summary>
        /// <param name="taskImageId">The ID of the image task to delete</param>
        /// <param name="companyId">Optional: The company ID for the task. If not provided, a denser query is performed to get this information. If incorrect, image might not be able to be retrieved. If you have it to hand, supply it.</param>
        Task DeleteAsync(int taskImageId, int? companyId = null);

    }
}
