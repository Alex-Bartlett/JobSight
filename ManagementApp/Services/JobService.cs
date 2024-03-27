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
            CheckForAuthorizationViolations(job, user);
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

        /// <summary>
        /// Checks if the user's current company matches the job's company. If not, throws an exception.
        /// </summary>
        /// <param name="job">The job to check</param>
        /// <param name="user">The current user</param>
        /// <exception cref="UnauthorizedAccessException">Thrown if the user is not allowed to access the job.</exception>
        private void CheckForAuthorizationViolations(Job? job, User user)
        {
            if (job is null)
            {
                return;
            }
            if (user.CurrentCompanyId != job.CompanyId)
            {
                _logger.LogWarning("User tried to access unauthorized resource.", [job.Id, user.Id]);
                throw new UnauthorizedAccessException("User is not authorized to access this job.");
            }
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

        public void DeleteAsync(int jobId)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> IsValid(Job job)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user is null)
            {
                _logger.LogError("User is null. Cannot validate job.", [job]);
                return false;
            }
            else if (user.CurrentCompanyId is null)
            {
                _logger.LogError("Current company is null. Cannot validate job.", [job, user]);
                return false;
            }

            var customers = await _customerService.GetAllAsync(user.CurrentCompanyId.Value);
            if (customers is null)
            {
                _logger.LogError("Current company has no customers. Cannot validate job.", [job, user]);
                return false;
            }

            // If customerId is not in the customer list of the company, throw an exception
            if (!customers.Any(c => c.Id == job.CustomerId))
            {
                _logger.LogError("Customer does not belong to current company. Potential overposting attempt.", [job, user]);
                return false;
            }

            if (job.CompanyId != user.CurrentCompanyId)
            {
                _logger.LogError("Job does not belong to current company. Potential overposting attempt.", [job, user]);
                return false;
            }

            return true;
        }
    }
}
