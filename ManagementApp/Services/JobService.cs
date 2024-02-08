using Shared.Models;

namespace ManagementApp.Services
{
    public class JobService : IJobService
    {
        private readonly ApiDataService _apiDataService;
        public List<Job> Jobs { get; set; }
        public JobService(ApiDataService apiDataService) 
        {
            _apiDataService = apiDataService;
        }
        public async Task<List<Job>?> GetAllJobs()
        {
            List<Job>? jobs = await _apiDataService.SendQuery<List<Job>>(HttpRequestMethod.GET, "/api/jobs");

            // If no jobs are found, return an empty array
            if (jobs is null)
            {
                return [];
            }
            return jobs;
        }

        public async Task<Job?> GetJob(int jobId)
        {
            Job? job = await _apiDataService.SendQuery<Job>(HttpRequestMethod.GET, $"/api/job/{jobId}");
            return job;
        }
    }
}
