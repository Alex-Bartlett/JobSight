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

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            var currentCompany = _companyService.CurrentCompany;
            if (currentCompany is not null)
            {
                return await _jobRepository.GetAllAsync(currentCompany.Id);
            }
            else
            {
                return new List<Job>();
            }
        }

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
            {
                _logger.LogWarning($"Job could not be found.", [jobId]);
            }
            return job;
        }
    }
}
