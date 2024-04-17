using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Globalization;

namespace Shared.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly JobSightDbContext _context;
        public TaskRepository(JobSightDbContext context)
        {
            _context = context;
        }
        public async Task<JobTask?> AddAsync(JobTask task)
        {
            _context.JobTasks.Add(task);
            await _context.SaveChangesAsync();
            await _context.Entry(task).ReloadAsync();
            return task;
        }

        public async Task DeleteAsync(int id)
        {
			// A more efficient way to delete entities https://khalidabuhakmeh.com/more-efficient-deletes-with-entity-framework-core#entity-framework-core-7-updates
			await _context.JobTasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<JobTask>> GetAllAsync(int jobId)
        {
            return await _context.JobTasks
                .Where(task => task.JobId == jobId)
                .Include(task => task.Images)
                .Include(task => task.User)
                .ToListAsync();
        }

        public async Task<JobTask?> GetByIdAsync(int id)
        {
            return await _context.JobTasks
                .Where(task => task.Id == id)
                .Include(task => task.Job)
                .Include(task => task.Images)
                .Include(task => task.User)
                .SingleOrDefaultAsync();
        }

        public async Task<JobTask?> UpdateAsync(JobTask updatedTask)
        {
            var task = _context.JobTasks.Find(updatedTask.Id);

            if (task is null)
            {
                return null;
            }

            task.Description = updatedTask.Description;
            task.JobId = updatedTask.JobId;
            task.UserId = updatedTask.UserId;
            task.StartDateTime = updatedTask.StartDateTime;
            task.EndDateTime = updatedTask.EndDateTime;

            await _context.SaveChangesAsync();
            return task;
        }
    }
}