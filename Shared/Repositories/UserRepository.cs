using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace Shared.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JobSightDbContext _context;
        public UserRepository(JobSightDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserWithNavigationsByIdAsync(string id)
        {
            var result = await _context.Users
                .Include(u => u.CurrentCompany)
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
            return result;
        }
    }
}