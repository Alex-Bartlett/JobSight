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
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ICompanyService> _companyServiceMock = new();
        private readonly Mock<ILogger<CustomerService>> _loggerMock = new();

        public CustomerServiceTests()
        {
            _sut = new CustomerService(_customerRepositoryMock.Object, _userServiceMock.Object, _companyServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            int expectedId = 1;
            int companyId = 1;
            Customer stubCustomer = new()
            {
                Id = expectedId,
                CompanyId = companyId
            };
            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(expectedId)).ReturnsAsync(stubCustomer);

            // Act
            var result = await _sut.GetByIdAsync(expectedId, stubUser);

            // Assert
            Assert.Equal(expectedId, result?.Id);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            int customerId = 1;
            User stubUser = new()
            {
                CurrentCompanyId = 1
            };

            // Act
            var result = await _sut.GetByIdAsync(customerId, stubUser);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetByIdAsync_ShouldThrowException_WhenCurrentCompanyDoesNotMatch()
        {
            // Arrange
            int customerId = 1;
            int companyId = 1;
            int wrongCompanyId = 2;
            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            User stubUser = new()
            {
                CurrentCompanyId = wrongCompanyId
            };

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(stubCustomer);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.GetByIdAsync(customerId, stubUser));
        }

        [Fact]
        public async void GetByIdAsync_ShouldThrowException_WhenCurrentCompanyIsNull()
        {
            // Arrange
            int customerId = 1;
            int companyId = 1;
            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            User stubUser = new()
            {
                CurrentCompanyId = null
            };

            _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(stubCustomer);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.GetByIdAsync(customerId, stubUser));
        }

        [Fact]
        public async void GetAllAsync_ShouldOnlyReturnCompanysCustomers_WhenCustomersExist()
        {
            // Arrange
            int expectedCompanyId = 1;
            int expectedCustomerId = 1;

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
                Id = expectedCustomerId,
                CompanyId = expectedCompanyId,
            };

            Customer unexpectedStubCustomer = new()
            {
                Id = 2,
                CompanyId = fakeCompany.Id,
            };

            // All customers in mock database
            List<Customer> stubCustomers = new()
            {
                expectedStubCustomer1,
                unexpectedStubCustomer,
            };

            _customerRepositoryMock.Setup(x => x.GetAllAsync(expectedCompanyId)).ReturnsAsync(stubCustomers);

            // Act
            var result = await _sut.GetAllAsync(expectedCustomerId);
            var resultId = result.First().Id;

            // Assert
            Assert.Equal(expectedCustomerId, resultId);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNewCustomer_WhenCustomerIsValid()
        {
            // Arrange
            int companyId = 1;

            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer mockCustomer = new()
            {
                Name = "Test Customer",
                CompanyId = companyId
            };

            Customer mockCustomerComplete = new()
            {
                Id = 1,
                Name = "Test Customer",
                CompanyId = companyId
            };

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerRepositoryMock.Setup(x => x.AddAsync(mockCustomer)).ReturnsAsync(mockCustomerComplete);

            // Act
            var result = await _sut.CreateAsync(mockCustomer);
            mockCustomer.Id = result!.Id;

            // Assert
            Assert.Equivalent(mockCustomer, result, strict: true);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenUserIsNull()
        {
            // Arrange
            int companyId = 1;

            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer mockCustomer = new()
            {
                Name = "Test Customer",
                CompanyId = companyId
            };

            Customer mockCustomerComplete = new()
            {
                Id = 1,
                Name = "Test Customer",
                CompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(() => null);
            _customerRepositoryMock.Setup(x => x.AddAsync(mockCustomer)).ReturnsAsync(mockCustomerComplete);

            // Act
            var result = await _sut.CreateAsync(mockCustomer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenCurrentCompanyIsNull()
        {
            // Arrange
            int companyId = 1;

            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer mockCustomer = new()
            {
                Name = "Test Customer",
                CompanyId = companyId
            };

            Customer mockCustomerComplete = new()
            {
                Id = 1,
                Name = "Test Customer",
                CompanyId = companyId
            };

            User stubUser = new()
            {
                CurrentCompanyId = null
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerRepositoryMock.Setup(x => x.AddAsync(mockCustomer)).ReturnsAsync(mockCustomerComplete);

            // Act
            var result = await _sut.CreateAsync(mockCustomer);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenCustomerCompanyDoesNotMatchCurrentCompany()
        {
            // Arrange
            int companyId = 1;
            int wrongCompanyId = 2;

            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer mockCustomer = new()
            {
                Name = "Test Customer",
                CompanyId = wrongCompanyId
            };

            Customer mockCustomerComplete = new()
            {
                Id = 1,
                Name = mockCustomer.Name,
                CompanyId = mockCustomer.CompanyId
            };

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerRepositoryMock.Setup(x => x.AddAsync(mockCustomer)).ReturnsAsync(mockCustomerComplete);

            // Act
            var result = await _sut.CreateAsync(mockCustomer);

            // Assert
            Assert.Null(result);
        }
    }
}
