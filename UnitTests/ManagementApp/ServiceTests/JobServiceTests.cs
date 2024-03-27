using Microsoft.Extensions.Logging;
using ManagementApp.Services;
using Moq;
using Shared.Repositories;
using Shared.Models;
using System.ComponentModel.Design;
using Castle.Core.Resource;

namespace UnitTests.ManagementApp.ServiceTests
{
    public class JobServiceTests
    {
        private readonly JobService _sut;
        private readonly Mock<IJobRepository> _jobRepositoryMock = new();
        private readonly Mock<ICompanyService> _companyServiceMock = new();
        private readonly Mock<ICustomerService> _customerServiceMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILogger<JobService>> _jobServiceLoggerMock = new();
        public JobServiceTests()
        {
            _sut = new JobService(_jobRepositoryMock.Object, _companyServiceMock.Object, _customerServiceMock.Object, _userServiceMock.Object, _jobServiceLoggerMock.Object);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnJob_WhenJobExists()
        {
            // Arrange
            int jobId = 0;
            int companyId = 0;
            Job stubJob = new()
            {
                Id = jobId,
                CompanyId = companyId,
            };
            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };
            _jobRepositoryMock.Setup(x => x.GetByIdAsync(jobId))
                .ReturnsAsync(stubJob);

            // Act
            var job = await _sut.GetByIdAsync(jobId, stubUser);

            // Assert
            Assert.Equal(jobId, job?.Id);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnNull_WhenJobDoesNotExist()
        {
            // Arrange
            int jobId = 0;
            User stubUser = new();

            // Act
            var job = await _sut.GetByIdAsync(jobId, stubUser);

            // Assert
            Assert.Null(job);
        }

        [Fact]
        public async void GetByIdAsync_ShouldThrowException_WhenCurrentCompanyIsNull()
        {
            // Arrange
            int jobId = 0;
            int companyId = 0;
            User stubUser = new();
            Job stubJob = new()
            {
                Id = jobId,
                CompanyId = companyId,
            };

            _jobRepositoryMock.Setup(x => x.GetByIdAsync(jobId)).ReturnsAsync(stubJob);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.GetByIdAsync(jobId, stubUser));
        }

        [Fact]
        public async void GetByIdAsync_ShouldThrowException_WhenCurrentCompanyDoesNotMatch()
        {
            // Arrange
            int jobId = 0;
            int companyId = 0;
            int wrongCompanyId = 1;
            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };
            Job stubJob = new()
            {
                Id = jobId,
                CompanyId = wrongCompanyId
            };

            _jobRepositoryMock.Setup(x => x.GetByIdAsync(jobId)).ReturnsAsync(stubJob);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.GetByIdAsync(jobId, stubUser));
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnMany_WhenJobsExist()
        {
            // Arrange
            int companyId = 0;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            IEnumerable<Job> stubJobs = new List<Job>
            {
                new Job { Id = 1, Company = stubCompany},
                new Job { Id = 2, Company = stubCompany},
                new Job { Id = 3, Company = stubCompany},
            };

            _jobRepositoryMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(stubJobs);

            // Act
            var jobs = await _sut.GetAllAsync(companyId);

            // Assert
            Assert.Equal(stubJobs, jobs);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyList_WhenNoJobsExist()
        {
            // Arrange
            int companyId = 1;
            Company stubCompany = new()
            {
                Id = companyId
            };

            IEnumerable<Job> stubJobs = new List<Job>();

            // Act
            var jobs = await _sut.GetAllAsync(companyId);

            // Assert
            Assert.Empty(jobs);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNewJob_WhenJobIsValid()
        {
            // Arrange
            int companyId = 1;
            int customerId = 1;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = customerId,
                CompanyId = companyId,
                Customer = stubCustomer,
                Company = stubCompany
            };

            Job stubJob = new()
            {
                Id = 1,
                Reference = mockJob.Reference,
                CustomerId = mockJob.CustomerId,
                CompanyId = mockJob.CompanyId,
                Customer = mockJob.Customer,
                Company = mockJob.Company
            };

            List<Customer> mockCustomerList = [stubCustomer];

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerServiceMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(mockCustomerList);
            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);
            mockJob.Id = resultJob!.Id; // Update the mockJob with the generated ID (needed for object comparison in assertion)

            // Assert
            Assert.Equivalent(mockJob, resultJob, strict: true);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenUserIsNull()
        {
            // Arrange
            int companyId = 1;
            int customerId = 1;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = customerId,
                CompanyId = companyId,
                Customer = stubCustomer,
                Company = stubCompany
            };

            Job stubJob = new()
            {
                Id = 1,
                Reference = mockJob.Reference,
                CustomerId = mockJob.CustomerId,
                CompanyId = mockJob.CompanyId,
                Customer = mockJob.Customer,
                Company = mockJob.Company
            };

            List<Customer> mockCustomerList = [stubCustomer];

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(() => null);
            _customerServiceMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(mockCustomerList);
            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);

            // Assert
            Assert.Null(resultJob);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenCurrentCompanyIsNull()
        {
            // Arrange
            int companyId = 1;
            int customerId = 1;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = customerId,
                CompanyId = companyId,
                Customer = stubCustomer,
                Company = stubCompany
            };

            Job stubJob = new()
            {
                Id = 1,
                Reference = mockJob.Reference,
                CustomerId = mockJob.CustomerId,
                CompanyId = mockJob.CompanyId,
                Customer = mockJob.Customer,
                Company = mockJob.Company
            };

            List<Customer> mockCustomerList = [stubCustomer];

            User stubUser = new()
            {
                CurrentCompanyId = null
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerServiceMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(mockCustomerList);
            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);

            // Assert
            Assert.Null(resultJob);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenCurrentCompanyHasNoCustomers()
        {
            // Arrange
            int companyId = 1;
            int customerId = 1;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = customerId,
                CompanyId = companyId,
                Customer = stubCustomer,
                Company = stubCompany
            };

            Job stubJob = new()
            {
                Id = 1,
                Reference = mockJob.Reference,
                CustomerId = mockJob.CustomerId,
                CompanyId = mockJob.CompanyId,
                Customer = mockJob.Customer,
                Company = mockJob.Company
            };

            List<Customer> mockCustomerList = [];

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerServiceMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(mockCustomerList);
            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);

            // Assert
            Assert.Null(resultJob);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenCustomerDoesNotBelongToCurrentCompany()
        {
            // Arrange
            int companyId = 1;
            int customerId = 1;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer invalidCustomer = new()
            {
                Id = customerId,
                CompanyId = 2,
            };

            Customer validCustomer = new()
            {
                Id = 2,
                CompanyId = companyId
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = customerId,
                CompanyId = companyId,
                Customer = invalidCustomer,
                Company = stubCompany
            };

            Job stubJob = new()
            {
                Id = 1,
                Reference = mockJob.Reference,
                CustomerId = mockJob.CustomerId,
                CompanyId = mockJob.CompanyId,
                Customer = mockJob.Customer,
                Company = mockJob.Company
            };

            List<Customer> mockCustomerList = [validCustomer];

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerServiceMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(mockCustomerList);
            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);

            // Assert
            Assert.Null(resultJob);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnNull_WhenJobCompanyDoesNotMatchCurrentCompany()
        {
            // Arrange
            int companyId = 1;
            int wrongCompanyId = 2;
            int customerId = 1;
            Company stubCompany = new()
            {
                Id = companyId,
            };

            Customer stubCustomer = new()
            {
                Id = customerId,
                CompanyId = companyId
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = customerId,
                CompanyId = wrongCompanyId,
                Customer = stubCustomer,
                Company = stubCompany
            };

            Job stubJob = new()
            {
                Id = 1,
                Reference = mockJob.Reference,
                CustomerId = mockJob.CustomerId,
                CompanyId = mockJob.CompanyId,
                Customer = mockJob.Customer,
                Company = mockJob.Company
            };

            List<Customer> mockCustomerList = [stubCustomer];

            User stubUser = new()
            {
                CurrentCompanyId = companyId
            };

            _userServiceMock.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(stubUser);
            _customerServiceMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(mockCustomerList);
            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);

            // Assert
            Assert.Null(resultJob);
        }
    }
}