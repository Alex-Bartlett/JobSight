

using Shared.Models;

namespace Shared.Repositories
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Get a single company by id
        /// </summary>
        /// <param name="id">Company id</param>
        /// <returns>Company with matching id, or null if not found</returns>
        Task<Company?> GetByIdAsync(int id);
        Task<IEnumerable<Company>> GetAllAsync();
        Task AddAsync(Company job);
        Task UpdateAsync(Company job);
        Task DeleteAsync(int id);
    }
}