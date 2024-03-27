using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync(int companyId);
        /// <summary>
        /// Gets a single customer
        /// </summary>
        /// <param name="id">The id of the customer to get</param>
        /// <param name="user">The current user</param>
        /// <returns>The customer if found, or null if not found</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the customer does not belong to the current user's company</exception>
        Task<Customer?> GetByIdAsync(int id, User user);
        Task<Customer?> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}
