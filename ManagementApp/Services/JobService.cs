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

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job is null)
            {
                _logger.LogWarning("Job could not be found.", [jobId]);
            }
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
