using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly ILogger _logger;

        public CustomerService(ICustomerRepository customerRepository, IUserService userService, ICompanyService companyService, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _companyService = companyService;
            _logger = logger;
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
            // This should return an empty list if none exist
            return await _customerRepository.GetAllAsync(companyId);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            // This needs to be implemented in a general sense. Can't have every method check if the user is validated.
            var currentCompanyId = await _userService.GetCurrentCompanyIdAsync();
            if (currentCompanyId is null)
            {
                _logger.LogError("User is not logged in");
            }
            return new Customer();
        }

        public Task<Customer?> UpdateAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
