using Shared.Models;

namespace ManagementApp.Services
{
    public interface ITaskImageService
    {
        /// <summary>
        /// Adds a new task image and uploads the image file to the storage service.
        /// </summary>
        /// <param name="jobTaskImage">The task image object to add</param>
        /// <param name="imgStream">The file stream for the incoming image</param>
        /// <returns>The new task image object</returns>
        public Task<JobTaskImage?> AddImage(JobTaskImage jobTaskImage, MemoryStream imgStream);

        public Task<IEnumerable<JobTaskImage>> RefreshImageUrls(IEnumerable<JobTaskImage> images, int companyId);
    }
}
