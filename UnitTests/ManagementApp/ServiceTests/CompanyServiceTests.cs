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
    public class CompanyServiceTests
    {
        private readonly CompanyService _sut;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<ILogger<CompanyService>> _loggerMock = new();

        public CompanyServiceTests()
        {
            _sut = new CompanyService(_companyRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void ChangeCompany_ShouldUpdateCompanyProperty_WhenCompanyExists()
        {
            // Arrange
            int companyId = 0;
            Company mockCompany = new()
            {
                Id = companyId,
            };
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(companyId)).ReturnsAsync(mockCompany);

            // Act
            await _sut.ChangeCompany(companyId);
            var actualId = _sut.Company?.Id;

            // Assert
            Assert.Equal(companyId, actualId);
        }

        [Fact]
        public async void ChangeCompany_ShouldReturnCompany_WhenCompanyExists()
        {
            // Arrange
            int companyId = 0;
            Company mockCompany = new()
            {
                Id = companyId,
            };
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(companyId)).ReturnsAsync(mockCompany);

            // Act
            var result = await _sut.ChangeCompany(companyId);

            // Assert
            Assert.Equal(companyId, result?.Id);
        }
    }
}
