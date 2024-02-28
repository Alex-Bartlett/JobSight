using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace Shared.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JobSightDbContext _context;
        private readonly ILogger _logger;
        public UserRepository(JobSightDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


    }
}