﻿using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IJobService _jobService;
        private readonly ITaskImageService _taskImageService;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IUserService userService, IJobService jobService, ITaskImageService taskImageService)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _userService = userService;
            _jobService = jobService;
            _taskImageService = taskImageService;
        }

        public async Task<JobTask?> CreateAsync(JobTask task)
        {
            if (!await IsValid(task))
            {
                _logger.LogError("Task is not valid.", [task]);
                return null;
            }

            var newTask = await _taskRepository.AddAsync(task);

            if (newTask is null)
            {
                _logger.LogError("Task could not be created.", [task]);
            }

            return newTask;
        }

        public async Task DeleteAsync(int id, User user)
        {
            // Get the task first to perform authorisation checks. Costly, but necessary.
            var entityToDelete = await GetByIdAsync(id, user);
            if (entityToDelete is null)
            {
                _logger.LogWarning("Failed to delete task.");
                return;
            }

            // Though entityToDelete.Id not strictly the same id as the one passed in,
            // I'd rather delete the entity that was authorised than one that wasn't.
            await _taskRepository.DeleteAsync(entityToDelete.Id);
        }

        public async Task<IEnumerable<JobTask>> GetAllAsync(int jobId, bool refreshImageUrls = true)
        {
            // Worst case, this will return an empty list. No error handling is needed.
            var tasks = await _taskRepository.GetAllAsync(jobId);
            if (refreshImageUrls)
            {
                var tasksArray = tasks.ToArray();
                for (int i = 0; i < tasksArray.Length; i++)
                {
                    tasksArray[i] = await UpdateUrls(tasksArray[i]);
                }
                return tasksArray;
            }
            else {
                return tasks;
            }
        }

        public async Task<JobTask?> GetByIdAsync(int taskId, User user, bool refreshImageUrls = true)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task is null)
            {
                _logger.LogWarning("Task could not be found.", [taskId]);
                return null;
            }
            else if (task.Job is null || task.Job.CompanyId is null)
            {
                // Job is included when fetching a single task.
                // Access validation cannot be performed without a job or company, so return null.
                _logger.LogWarning("Task's job or company could not be found.", [task]);
                return null;
            }
            // If authorization checks fail, an exception will be thrown. Task will not be returned.
            AccessValidation.CheckForAuthorizationViolations(task.Job.CompanyId.Value, user, _logger);

            if (refreshImageUrls)
            {
                return await UpdateUrls(task);
            }
            else
            {
                return task;
            }
        }

        public async Task<JobTask?> UpdateAsync(JobTask task)
        {
            if (!await IsValid(task))
            {
                _logger.LogError("Task is not valid.", [task]);
                return null;
            }

            var updatedTask = await _taskRepository.UpdateAsync(task);

            if (updatedTask is null)
            {
                _logger.LogError("Task could not be updated", [task]);
            }

            return updatedTask;
        }

        private async Task<bool> IsValid(JobTask task)
        {
            var user = await _userService.GetCurrentUserAsync();
            return AccessValidation.IsValid(task, user, _logger);
        }

        private async Task<JobTask> UpdateUrls(JobTask task) 
        {
            if (task.Images.Count == 0)
            {
                // Dont bother trying to update image urls if there are no images.
                return task;
            }
            if (task.Job?.CompanyId is null)
            {
                _logger.LogWarning("Could not update image urls because task's job or company could not be found.", [task]);
                return task;
            }
            var newImages = await _taskImageService.RefreshImageUrls(task.Images, task.Job.CompanyId.Value);
            task.Images = newImages.ToList();
            return task;
        }
    }
}
