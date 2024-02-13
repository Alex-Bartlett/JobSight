using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Repositories
{
    public interface IJobRepository
    {
        Task<Job> GetByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllAsync(int companyId);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(int id);

    }
}
