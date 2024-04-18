using Infrastructure;
using Shared.Models;

namespace Shared.Repositories
{
    public class TaskImageRepository : ITaskImageRepository
    {
        private readonly JobSightDbContext _context;
        private readonly ImageBucketConnector _supabaseConnector;

        public TaskImageRepository(JobSightDbContext context, ImageBucketConnector supabaseConnector)
        {
            _context = context;
            _supabaseConnector = supabaseConnector;
        }

        public async Task<JobTaskImage?> AddAsync(JobTaskImage taskImage, MemoryStream imgStream, int companyId, int expirationInMinutes)
        {
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(expirationInMinutes);
            var imageName = await _supabaseConnector.AddImage(companyId, imgStream);
            var imageUrl = await _supabaseConnector.CreateSignedUrl(companyId, imageName, expirationInMinutes);

            taskImage.ImageName = imageName;
            taskImage.ImageUrl = imageUrl;
            taskImage.UrlExpiry = expirationTime;

            _context.JobTaskImages.Add(taskImage);
            await _context.SaveChangesAsync();
            await _context.Entry(taskImage).ReloadAsync();

            return taskImage;
        }

        public Task DeleteAsync(int taskImageId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobTaskImage>> GetAllAsync(int taskId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<JobTaskImage?> GetByIdAsync(string taskImageId, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}