using Microsoft.AspNetCore.Identity;
using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private Company? CurrentCompany;

        public CompanyService(ICompanyRepository companyRepository, IUserService userService, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyRepository.GetAllAsync();
        }

        public async Task<Company?> GetCurrentCompanyAsync()
        {
            if (CurrentCompany is null)
            {
                // Try to set currentCompany using the user's currentCompanyId
                await UpdateCurrentCompanyFromUser();
                if (CurrentCompany is null)
                {
                    _logger.LogWarning("Current company has not been set and could not be automatically set.");
                }
            }
            return CurrentCompany;
        }

        public async Task<Company?> UpdateCurrentCompanyAsync(int companyId)
        {
            Company? newCompany = await _companyRepository.GetByIdAsync(companyId);
            if (newCompany is not null)
            {
                CurrentCompany = newCompany;
            }
            else
            {
                _logger.LogWarning("Company could not be found.", [companyId]);
            }
            return CurrentCompany;
        }

        /// <summary>
        /// Gets CompanyId from the currently logged in user
        /// </summary>
        /// <returns>User's company id</returns>
        private async Task<int?> GetCompanyIdFromUser()
        {
            var currentUser = await _userService.GetCurrentUserAsync();

            return currentUser?.CurrentCompanyId;
        }

        private async Task UpdateCurrentCompanyFromUser()
        {
            var companyId = await GetCompanyIdFromUser();
            if (companyId is not null)
            {
                var res = await UpdateCurrentCompanyAsync(companyId.Value);
            }
        }
    }
}
