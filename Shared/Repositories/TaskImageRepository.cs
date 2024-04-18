using Infrastructure;
using Shared.Models;

namespace Shared.Repositories
{
    public class TaskImageRepository : ITaskImageRepository
    {
        private readonly JobSightDbContext _context;
        private readonly SupabaseConnector _supabaseConnector;

        public TaskImageRepository(JobSightDbContext context, SupabaseConnector supabaseConnector)
        {
            _context = context;
            _supabaseConnector = supabaseConnector;
        }

        public async Task<JobTaskImage?> AddAsync(JobTaskImage taskImage, MemoryStream imgStream, int companyId)
        {
            await _supabaseConnector.AddImage(companyId, imgStream);
            return null;
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