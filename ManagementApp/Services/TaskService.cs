using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IJobService _jobService;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IUserService userService, IJobService jobService)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _userService = userService;
            _jobService = jobService;
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

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobTask>> GetAllAsync(int jobId)
        {
            // Worst case, this will return an empty list. No error handling is needed.
            return _taskRepository.GetAllAsync(jobId);
        }

        public async Task<JobTask?> GetByIdAsync(int taskId, User user)
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

            return task;
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
    }
}
