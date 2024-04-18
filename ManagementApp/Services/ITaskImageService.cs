using Shared.Models;

namespace ManagementApp.Services
{
    public interface ITaskImageService
    {
        public Task<JobTaskImage?> AddImage(int companyId, MemoryStream imgStream);
    }
}
