using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Globalization;

namespace Shared.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly JobSightDbContext _context;
        private readonly ImageBucketConnector _imageBucketConnector;
        public TaskRepository(JobSightDbContext context, ImageBucketConnector imageBucketConnector)
        {
            _context = context;
            _imageBucketConnector = imageBucketConnector;
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
            var task = await _context.JobTasks.FindAsync(id);

            if (task is null)
            {
                return;
            }

            // Remove the images from the database (task image rows are deleted by cascade)
            if (task.Job?.CompanyId is not null && task.Images.Count > 0)
            {
                var imageUrls = task.Images
                    .Where(image => image.ImageName is not null)
                    .Select(image => image.ImageName)
                    .ToList();
                await _imageBucketConnector.DeleteImages(task.Job.CompanyId.Value, imageUrls!); // Nullability checked in above line
            }

            _context.JobTasks.Remove(task);
            await _context.SaveChangesAsync();

			// A more efficient way to delete entities https://khalidabuhakmeh.com/more-efficient-deletes-with-entity-framework-core#entity-framework-core-7-updates
			/*await _context.JobTasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();*/
            // Keeping this for future reference, but it's benefits don't exist as object is already loaded for image removal
        }

        public async Task<IEnumerable<JobTask>> GetAllAsync(int jobId)
        {
            var tasks = await _context.JobTasks
                .Where(task => task.JobId == jobId)
                .Include(task => task.Images)
                .Include(task => task.User)
                .ToListAsync();
            return tasks;
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