using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class TaskImageService : ITaskImageService
    {
        private readonly ITaskImageRepository _taskImageRepository;
        private readonly IConfiguration _configuration;

        public TaskImageService(ITaskImageRepository taskImageRepository, IConfiguration configuration)
        {
            _taskImageRepository = taskImageRepository;
            _configuration = configuration;

        }

        public async Task<JobTaskImage?> AddImage(JobTaskImage jobTaskImage, int companyId, MemoryStream imgStream)
        {
            var expirationTime = _configuration.GetValue<int>("ImageUploadConfig:UrlExpirationInMinutes");
            var result = await _taskImageRepository.AddAsync(jobTaskImage, imgStream, companyId, expirationTime);
            return result;
        }
    }
}
