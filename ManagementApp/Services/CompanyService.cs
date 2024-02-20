using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger _logger;

        public CompanyService(ICompanyRepository companyRepository, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }
        public Company? CurrentCompany { get; private set; }

        public async Task<Company?> ChangeCompany(int companyId)
        {
            Company? newCompany = await _companyRepository.GetByIdAsync(companyId);
            if (newCompany is not null)
            {
                CurrentCompany = newCompany;
            }
            else {
                _logger.LogWarning("Company could not be found.", [companyId]);
            }
            return newCompany;
        }
    }
}
