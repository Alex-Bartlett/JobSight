using Shared.Models;

namespace ManagementApp.Services
{
    public interface IUserService
    {
        public Task<Company?> ChangeCurrentCompany(int companyId);
        public Task<User?> GetCurrentUserAsync();
        public Task UpdateCurrentUser();
    }
}
