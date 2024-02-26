using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> ChangeCurrentCompanyAsync(User user, int companyId);
        public Task<bool> UserBelongsToCompanyAsync(User user, int companyId);
    }
}
