using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace Shared.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JobSightDbContext _context;
        private readonly ILogger _logger;
        public UserRepository(JobSightDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="companyId"></param>
        /// <returns>User if successful, null if user does not belong to company</returns>
        public async Task<User?> ChangeCurrentCompanyAsync(User user, int companyId)
        {
            bool belongsToCompany = await UserBelongsToCompanyAsync(user, companyId);

            if (!belongsToCompany)
            {
                // User does not belong to the requested company, therefore cannot be updated
                _logger.LogError("Could not change current company because user is not a member of the company.", [user, companyId]);
                return null;
            }

            user.CurrentCompanyId = companyId;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<bool> UserBelongsToCompanyAsync(User user, int companyId)
        {
            var result = await _context.UserCompanies.Where(x => x.UserId == user.Id && x.CompanyId == companyId).SingleOrDefaultAsync();
            // If user is null, return false. Else return true
            return result is not null;
        }
    }
}