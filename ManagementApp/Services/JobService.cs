using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private ICompanyService _companyService;
        private readonly ILogger _logger;

        public JobService(IJobRepository jobRepository, ICompanyService companyService, ILogger<JobService> logger)
        {
            _jobRepository = jobRepository;
            _companyService = companyService;
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
            var newJob = await _jobRepository.AddAsync(job);

            if (job is null)
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

        public Task<Job?> UpdateAsync(Job job)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(int jobId)
        {
            throw new NotImplementedException();
        }
    }
}
