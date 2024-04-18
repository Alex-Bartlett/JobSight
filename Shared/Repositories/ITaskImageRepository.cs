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
        /// <param name="companyId">The company ID for the task, used to locate the relevant bucket</param>
        /// <returns>The task image object</returns>
        Task<JobTaskImage?> GetByIdAsync(string taskImageId, int companyId);

        /// <summary>
        /// Gets all image tasks for a given task.
        /// </summary>
        /// <param name="taskId">The id of the task to get the images of</param>
        /// <param name="companyId">The company ID for the task, used to locate the relevant bucket</param>
        /// <returns></returns>
        Task<IEnumerable<JobTaskImage>> GetAllAsync(int taskId, int companyId);

        /// <summary>
        /// Adds a new task image and uploads the image file to the storage service.
        /// </summary>
        /// <param name="taskImage">The task image object to add</param>
        /// <param name="imageFile">The relevant image file in bytes to add</param>
        /// <param name="companyId">The company ID for the task, used to locate the relevant bucket</param>
        /// <returns>The new task image object</returns>
        Task<JobTaskImage?> AddAsync(JobTaskImage taskImage, MemoryStream imgStream, int companyId);

        /// <summary>
        /// Deletes a task image from the database and the storage service.
        /// </summary>
        /// <param name="taskImageId">The ID of the image task to delete</param>
        /// <param name="companyId">The company ID for the task, used to locate the relevant bucket</param>
        Task DeleteAsync(int taskImageId, int companyId);

    }
}
