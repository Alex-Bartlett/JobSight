using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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


        public async Task<Job?> AddAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            // Get the new job
            await _context.Entry(job).ReloadAsync();
            return job;
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Job>> GetAllAsync(int companyId)
        {
            // if companyId is null, will this return all jobs for no company?
            return await _context.Jobs
                .Where(job => job.Company!.Id == companyId)
                .Include(job => job.Company)
                .Include(job => job.Customer)
                .ToListAsync();
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            // This should probably use FindAsync
            return await _context.Jobs
                .Where(job => job.Id == id)
                .Include(job => job.Company)
                .Include(job => job.Customer)
                .SingleOrDefaultAsync();
        }

        public async Task<Job?> UpdateAsync(Job updatedJob)
        {
            var job = await _context.Jobs.FindAsync(updatedJob.Id);

            if (job is null) // This needs logging in the service layer.
            {
                return null;
            }

            job.Description = updatedJob.Description;
            job.Address = updatedJob.Address;
            job.CustomerId = updatedJob.CustomerId;
            job.Reference = updatedJob.Reference;

            await _context.SaveChangesAsync();
            return job;
        }
    }
}
