using Microsoft.Extensions.Logging;
using ManagementApp.Services;
using Moq;
using Shared.Repositories;
using Shared.Models;

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

            _companyServiceMock.Setup(x => x.GetCurrentCompany()).Returns(stubCompany);
            _jobRepositoryMock.Setup(x => x.GetAllAsync(companyId)).ReturnsAsync(stubJobs);

            // Act
            var jobs = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(stubJobs, jobs);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyList_WhenNoJobsExist()
        {
            // Arrange
            Company stubCompany = new()
            {
                Id = 1,
            };

            IEnumerable<Job> stubJobs = new List<Job>();

            _jobRepositoryMock.Setup(x => x.GetAllAsync(stubCompany.Id)).ReturnsAsync(stubJobs);
            // Act
            var jobs = await _sut.GetAllAsync();

            // Assert
            Assert.Empty(jobs);
        }
    }
}