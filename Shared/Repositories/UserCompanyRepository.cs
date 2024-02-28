
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Repositories
{
    public class UserCompanyRepository : IUserCompanyRepository
    {
        private readonly JobSightDbContext _context;
        public UserCompanyRepository(JobSightDbContext context)
        {
            _context = context;
        }
        public async Task<bool> UserBelongsToCompanyAsync(string userId, int companyId)
        {
            var result = await _context.UserCompanies.Where(x => x.UserId == userId && x.CompanyId == companyId).SingleOrDefaultAsync();
            // If user is null, return false. Else return true
            return result is not null;
        }
    }
}