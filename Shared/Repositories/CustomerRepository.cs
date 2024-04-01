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

        public async Task<Customer?> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            // Get the new customer
            await _context.Entry(customer).ReloadAsync();
            return customer;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int companyId)
        {
            return await _context.Customers
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<Customer?> UpdateAsync(Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(updatedCustomer.Id);

            if (customer is null)
            {
                return null;
            }

            customer.Name = updatedCustomer.Name;
            customer.Address = updatedCustomer.Address;
            customer.Postcode = updatedCustomer.Postcode;
            // I can't see us ever wanting to change the company of a customer. But if we do, we'll need to add that here.

            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
