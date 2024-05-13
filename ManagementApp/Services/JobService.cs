using Shared.Models;
using Shared.Repositories;
using System.Security.Cryptography;

namespace ManagementApp.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private ICompanyService _companyService;
        private ICustomerService _customerService;
        private IUserService _userService;
        private readonly ILogger _logger;

        public JobService(IJobRepository jobRepository, ICompanyService companyService, ICustomerService customerService, IUserService userService, ILogger<JobService> logger)
        {
            _jobRepository = jobRepository;
            _companyService = companyService;
            _customerService = customerService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IEnumerable<Job>> GetAllAsync(int companyId)
        {
            return await _jobRepository.GetAllAsync(companyId);
        }

        public async Task<Job?> GetByIdAsync(int jobId, User user)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job is null)
            {
                _logger.LogWarning("Job could not be found.", [jobId]);
            }
            AccessValidation.CheckForAuthorizationViolations(job, user, _logger);
            return job;
        }

        public async Task<Job?> CreateAsync(Job job)
        {
            if (!await IsValid(job))
            {
                _logger.LogError("Job is not valid.", [job]);
                return null;
            }

            var newJob = await _jobRepository.AddAsync(job);

            if (newJob is null)
            {
                _logger.LogError("Job could not be created.", [job]);
            }

            return newJob;
        }

        public async Task<Job?> UpdateAsync(Job job)
        {
            if (!await IsValid(job))
            {
                _logger.LogError("Job is not valid.", [job]);
                return null;
            }

            var updatedJob = await _jobRepository.UpdateAsync(job);

            if (updatedJob is null)
            {
                _logger.LogError("Job could not be updated", [job]);
            }

            return updatedJob;
        }

        public async Task DeleteAsync(int jobId)
        {
            await _jobRepository.DeleteAsync(jobId);
        }

        private async Task<bool> IsValid(Job job)
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user is not null && user.CurrentCompanyId is not null)
            {
                var customers = await _customerService.GetAllAsync(user.CurrentCompanyId.Value);
                return AccessValidation.IsValid(job, user, _logger, customers);
            }
            else
            {
                // If user is null, use the generic IsValid method. An error is now expected, but is delivered properly.
                return AccessValidation.IsValid(job, user, _logger);
            }
        }
    }
}
