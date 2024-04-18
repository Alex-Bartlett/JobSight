using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class TaskImageService : ITaskImageService
    {
        private readonly ITaskImageRepository _taskImageRepository;

        public TaskImageService(ITaskImageRepository taskImageRepository)
        {
            _taskImageRepository = taskImageRepository;
        }

        public async Task<JobTaskImage?> AddImage(int companyId, MemoryStream imgStream)
        {
            await _taskImageRepository.AddAsync(new JobTaskImage(), imgStream, companyId);
            return null;
        }
    }
}
