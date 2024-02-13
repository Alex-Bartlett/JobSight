using Shared.Models;

namespace ManagementApp.Services
{
    public class CompanyService : ICompanyService
    {
        public Company Company { get; }

        public CompanyService()
        {
            Company = Set();
        }

        public Company Set()
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
