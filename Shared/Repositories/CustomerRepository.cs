using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly JobSightDbContext _context;

        public CustomerRepository(JobSightDbContext context)
        {
            _context = context;
        }

        public Task<Customer?> AddAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int companyId)
        {
            return await _context.Customers.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> UpdateAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
