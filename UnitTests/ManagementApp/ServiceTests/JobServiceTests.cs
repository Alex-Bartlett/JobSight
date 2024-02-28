using Microsoft.Extensions.Logging;
using ManagementApp.Services;
using Moq;
using Shared.Repositories;
using Shared.Models;
using System.ComponentModel.Design;

namespace UnitTests.ManagementApp.ServiceTests
{
    public class JobServiceTests
    {
        private readonly JobService _sut;
        private readonly Mock<IJobRepository> _jobRepositoryMock = new();
        private readonly Mock<ICompanyService> _companyServiceMock = new();
        private readonly Mock<ILogger<JobService>> _jobServiceLoggerMock = new();
        public JobServiceTests()
        {
            _sut = new JobService(_jobRepositoryMock.Object, _companyServiceMock.Object, _jobServiceLoggerMock.Object);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnJob_WhenJobExists()
        {
            // Arrange
            int jobId = 0;
            var stubJob = new Job
            {
                Id = jobId,
            };
            _jobRepositoryMock.Setup(x => x.GetByIdAsync(jobId))
                .ReturnsAsync(stubJob);

            // Act
            var job = await _sut.GetByIdAsync(jobId);

            // Assert
            Assert.Equal(jobId, job?.Id);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnNull_WhenJobDoesNotExist()
        {
            // Arrange
            int jobId = 0;

            // Act
            var job = await _sut.GetByIdAsync(jobId);

            // Assert
            Assert.Null(job);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnMany_WhenJobsExist()
        {
            // Arrange
            int companyId = 0;
            Company stubCompany = new Company
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
            Company stubCompany = new()
            {
                Id = 1,
            };

            Job mockJob = new()
            {
                Reference = "Test",
                CustomerId = 1,
                CompanyId = 1,
                Customer = new Customer(),
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

            _jobRepositoryMock.Setup(x => x.AddAsync(mockJob)).ReturnsAsync(stubJob);

            // Act
            var resultJob = await _sut.CreateAsync(mockJob);
            mockJob.Id = resultJob!.Id; // Update the mockJob with the generated ID (needed for object comparison in assertion)

            // Assert
            Assert.Equivalent(mockJob, resultJob, strict: true);
        }
    }
}