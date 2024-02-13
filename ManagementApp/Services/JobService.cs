using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ILogger _logger;
        public Company Company { get; }

        public JobService(IJobRepository jobRepository, ICompanyService company, ILogger<JobService> logger)
        {
            _jobRepository = jobRepository;
            _logger = logger;

            Company = company.Company;
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _jobRepository.GetAllAsync(Company.Id);
        }

        public async Task<Job> GetByIdAsync(int jobId)
        {
            _logger.LogInformation("Test!!");
            return await _jobRepository.GetByIdAsync(jobId);
        }
    }
}
