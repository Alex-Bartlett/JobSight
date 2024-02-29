using Shared.Models;

namespace ManagementApp.Services
{
    public interface IUserService
    {
        public Task<User?> GetCurrentUserAsync();

        /// <summary>
        /// Gets the current user object with references and collections populated (e.g. CurrentCompany)
        /// </summary>
        /// <returns>User if found, null if unsuccessful</returns>
        public Task<User?> GetCurrentUserWithNavigationsAsync();
        public Task UpdateCurrentUserAsync();
        /// <summary>
        /// A method to quickly get the current company id. Can be used as a shortcut for some methods. In regular practice, get the Company from GetCurrentUserAsync()
        /// </summary>
        /// <returns>The id of the user's current company</returns>
        public Task<int?> GetCurrentCompanyIdAsync();
        public Task<User?> ChangeCurrentCompanyAsync(int companyId);
    }
}
