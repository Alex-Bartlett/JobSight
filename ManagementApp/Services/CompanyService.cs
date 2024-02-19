using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public Company? Company { get; }

        public async Task<Company?> SetCompany(int id)
        {
            // TESTING ONLY
            return new Company()
            {
                Id = 2,
                AccountTier = new AccountTier()
                {
                    Id = 1,
                },
            };
        }
    }
}
