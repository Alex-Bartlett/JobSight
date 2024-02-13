using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        public Company company { get; }

        public JobService(IJobRepository jobRepository, ICompanyService company)
        {
            _jobRepository = jobRepository;
            this.company = company.Company;
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _jobRepository.GetAllAsync(company.Id);
        }

        public async Task<Job> GetByIdAsync(int jobId)
        {
            return await _jobRepository.GetByIdAsync(jobId);
        }
    }
}
