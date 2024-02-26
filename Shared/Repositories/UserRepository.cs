using Infrastructure;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User?> ChangeCurrentCompanyAsync(User user, int companyId)
        {
            bool belongsToCompany = await UserBelongsToCompanyAsync(user, companyId);

            if (!belongsToCompany)
            {
                // User does not belong to the requested company, therefore cannot be updated
                return null;
            }

            user.CurrentCompanyId = companyId;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<bool> UserBelongsToCompanyAsync(User user, int companyId)
        {
            var result = await _context.UserCompanies.Where(x => x.UserId == user.Id && x.CompanyId == companyId).SingleAsync();

            // If user is null, return false. Else return true
            return user is not null;
        }
    }
}