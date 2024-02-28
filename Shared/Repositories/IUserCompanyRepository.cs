
namespace Shared.Repositories
{
    public interface IUserCompanyRepository
    {
        public Task<bool> UserBelongsToCompanyAsync(string userId, int companyId);
    }
}