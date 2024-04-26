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

        public async Task<IEnumerable<JobTaskImage>> RefreshExpiredUrlsAsync(IEnumerable<JobTaskImage> jobTaskImages, int companyId, int expirationInMinutes)
        {
            // I feel conflicted on if this belongs here or in the service layer. This will do for now.
            var expiredImageIds = jobTaskImages.Where(image => image.UrlExpiry < DateTime.UtcNow).Select(image => image.Id);
            foreach (var image in jobTaskImages)
            {
                if (expiredImageIds.Contains(image.Id))
                {
                    var dbImage = await _context.JobTaskImages.FindAsync(image.Id);
                    if (dbImage is null)
                    {
                        // Probably should log this. Not sure if it's worth throwing an exception.
                        continue;
                    }
                    dbImage.ImageUrl = await _supabaseConnector.CreateSignedUrl(companyId, dbImage.ImageName!, expirationInMinutes); // Can't imagine ImageName will be null here. Might have to add a check later though just in case.
                    dbImage.UrlExpiry = DateTime.UtcNow.AddMinutes(expirationInMinutes);
                }
            }
            // Push the new URLs to the database
            await _context.SaveChangesAsync();
            return jobTaskImages;
        }
    }
}