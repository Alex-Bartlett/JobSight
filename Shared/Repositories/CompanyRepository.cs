using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly JobSightDbContext _context;

        public CompanyRepository(JobSightDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Company job)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies.Where(c => c.Id == id).SingleOrDefaultAsync();
        }

        public Task UpdateAsync(Company job)
        {
            throw new NotImplementedException();
        }
    }
}
