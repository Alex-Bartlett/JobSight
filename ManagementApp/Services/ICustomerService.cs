using Shared.Models;

namespace ManagementApp.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync(int companyId);
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}
