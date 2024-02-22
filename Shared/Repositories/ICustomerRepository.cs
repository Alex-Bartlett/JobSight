using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync(int companyId);
        Task<Customer?> AddAsync(Customer customer);
        Task<Customer?> UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}
