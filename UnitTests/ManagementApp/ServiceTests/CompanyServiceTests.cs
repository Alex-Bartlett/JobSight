using ManagementApp.Services;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        public async void ChangeCompany_ShouldUpdateCurrentCompany_WhenCompanyExists()
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
            var actualId = _sut.GetCurrentCompany()!.Id;

            // Assert
            Assert.Equal(companyId, actualId);
        }

        [Fact]
        public async void ChangeCompany_ShouldReturnNewCompany_WhenCompanyExists()
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

        [Fact]
        public async void ChangeCompany_ShouldReturnCurrentCompany_WhenNewCompanyDoesNotExist()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;

            Company mockOldCompany = new()
            {
                Id = oldCompanyId
            };
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(oldCompanyId)).ReturnsAsync(mockOldCompany);
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(() => null);

            // Act
            var oldCompany = await _sut.ChangeCompany(oldCompanyId);
            var newCompany = await _sut.ChangeCompany(newCompanyId);

            // Assert
            Assert.Equal(oldCompany, newCompany);
        }

        [Fact]
        public async void ChangeCompany_ShouldReturnNull_WhenNewCompanyDoesNotExistAndCurrentCompanyIsNull()
        {
            // Arrange
                // CurrentCompany is null on instantiation
            int newCompanyId = 0;
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(() => null);

            // Act
            var company = await _sut.ChangeCompany(newCompanyId);

            // Assert
            Assert.Null(company);
        }

        [Fact]
        public async void GetCurrentCompany_ShouldReturnCurrentCompany_WhenCurrentCompanyIsNotNull()
        {
            // Arrange
            int companyId = 0;
            Company mockCompany = new()
            {
                Id = companyId
            };
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(companyId)).ReturnsAsync(mockCompany);
            await _sut.ChangeCompany(companyId);
            // Act
            var company = _sut.GetCurrentCompany();
            // Assert
            Assert.Equal(companyId, company!.Id);
        }

        [Fact]
        public void GetCurrentCompany_ShouldReturnNull_WhenCurerntCompanyIsNull()
        {
            // Arrange
                // No setup necessary - CurrentCompany is null on instantiation
            // Act
            var company = _sut.GetCurrentCompany();

            // Assert
            Assert.Null(company);
        }
    }
}
