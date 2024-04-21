﻿using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class TaskImageService : ITaskImageService
    {
        private readonly ITaskImageRepository _taskImageRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TaskImageService(ITaskImageRepository taskImageRepository, ITaskRepository taskRepository, IConfiguration configuration, ILogger<TaskImageService> logger)
        {
            _taskImageRepository = taskImageRepository;
            _taskRepository = taskRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<JobTaskImage?> AddImage(JobTaskImage jobTaskImage, MemoryStream imgStream)
        {
            var task = await _taskRepository.GetByIdAsync(jobTaskImage.JobTaskId);

            int? companyId = task?.Job?.CompanyId;

            if (companyId is null)
            {
                _logger.LogError("Company ID could not be found for task image upload.");
                return null;
            }

            var expirationTime = _configuration.GetValue<int>("ImageUploadConfig:UrlExpirationInMinutes");
            var result = await _taskImageRepository.AddAsync(jobTaskImage, imgStream, companyId.Value, expirationTime);
            return result;
        }
    }
}
