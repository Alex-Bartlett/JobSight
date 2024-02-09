using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly JobSightDbContext _context;
        
        public JobRepository(JobSightDbContext context) 
        {
            _context = context;
        }


        public async Task AddJobAsync(Job job)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteJobAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all jobs
        /// </summary>
        /// <returns>An IEnumerable of jobs for company</returns>
        public async Task<IEnumerable<Job>> GetAllJobsAsync(int companyId)
        {
            // if companyId is null, will this return all jobs for no company?
            return await _context.Jobs.Where(job => job.Company!.Id == companyId).ToListAsync();
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.Where(job => job.Id == id).SingleAsync();
        }

        public async Task UpdateJobAsync(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
