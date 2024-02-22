using ManagementApp.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Models;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ManagementApp.ServiceTests
{
    public class CustomerServiceTests
    {

        private readonly CustomerService _sut;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
        private readonly Mock<ICompanyService> _companyServiceMock = new();
        private readonly Mock<ILogger<CustomerService>> _loggerMock = new();

        public CustomerServiceTests()
        {
            _sut = new CustomerService(_customerRepositoryMock.Object, _companyServiceMock.Object, _loggerMock.Object);
        }

        [Theory] // This needs to be a theory of expected ID and real id, with a pass and a fail
        public async void GetAllAsync_ShouldOnlyReturnCompanysCustomers_WhenCustomersExist()
        {
            // Arrange
            int expectedCompanyId = 1;

            Company stubCompany = new()
            {
                Id = expectedCompanyId,
            };

            Company fakeCompany = new()
            {
                Id = 2,
            };

            Customer expectedStubCustomer1 = new()
            {
                Id = 1,
                CompanyId = expectedCompanyId,
            };

            Customer expectedStubCustomer2 = new()
            {
                Id = 2,
                CompanyId = expectedCompanyId,
            };

            Customer unexpectedStubCustomer = new()
            {
                Id = 3,
                CompanyId = fakeCompany.Id,
            };

                // All customers in mock database
            List<Customer> stubCustomers = new()
            {
                expectedStubCustomer1,
                expectedStubCustomer2,
                unexpectedStubCustomer,
            };

                // Customers with the expected companyId 
            List<Customer> expectedCustomers = new()
            {
                expectedStubCustomer1,
                expectedStubCustomer2
            };

            _companyServiceMock.Setup(x => x.GetCurrentCompany()).Returns(stubCompany);
            _customerRepositoryMock.Setup(x => x.GetAllAsync(expectedCompanyId)).ReturnsAsync(stubCustomers);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(expectedCustomers, result);
        }
    }
}
