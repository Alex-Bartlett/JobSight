using Microsoft.Extensions.Logging;
using ManagementApp.Services;
using Moq;
using Shared.Repositories;
using Shared.Models;

namespace UnitTests.ServiceTests
{
    public class JobServiceTests
    {
        private readonly JobService _sut;
        private readonly Mock<IJobRepository> _jobRepositoryMock = new();
        private readonly Mock<ICompanyService> _companyServiceMock = new();
        private readonly Mock<ILogger<JobService>> _loggerMock = new();
        public JobServiceTests()
        {
            _sut = new JobService(_jobRepositoryMock.Object, _companyServiceMock.Object, _loggerMock.Object);
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
            Assert.Equal(jobId, job.Id);
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
            Company stubCompany = new Company
            {
                Id = 1,
            };

            IEnumerable<Job> stubJobs = new List<Job>
            {
                new Job{ Id = 1, Company = stubCompany},
                new Job{ Id = 2, Company = stubCompany},
                new Job{ Id = 3, Company = stubCompany},
            };

            /*            _companyServiceMock.Setup() // This whole test needs work. Authentication and identity needs to be set up first
            */
            _jobRepositoryMock.Setup(x => x.GetAllAsync(stubCompany.Id)).ReturnsAsync(stubJobs);

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