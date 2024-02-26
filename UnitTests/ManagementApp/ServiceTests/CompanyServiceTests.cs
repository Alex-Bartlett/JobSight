using ManagementApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Models;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace UnitTests.ManagementApp.ServiceTests
{
    public class CompanyServiceTests
    {
        private readonly CompanyService _sut;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILogger<CompanyService>> _loggerMock = new();

        public CompanyServiceTests()
        {
            _sut = new CompanyService(_companyRepositoryMock.Object, _userServiceMock.Object, _loggerMock.Object);
        }

        private Mock<UserManager<User>> GenerateUserManagerMock()
        {
            // https://stackoverflow.com/a/52562694/22966636
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            return userManager;
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
            await _sut.UpdateCurrentCompanyAsync(companyId);
            var currentCompany = await _sut.GetCurrentCompanyAsync();
            int actualId = currentCompany!.Id;

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
            var result = await _sut.UpdateCurrentCompanyAsync(companyId);

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
            var oldCompany = await _sut.UpdateCurrentCompanyAsync(oldCompanyId);
            var newCompany = await _sut.UpdateCurrentCompanyAsync(newCompanyId);

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
            var company = await _sut.UpdateCurrentCompanyAsync(newCompanyId);

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
            await _sut.UpdateCurrentCompanyAsync(companyId);
            // Act
            var company = await _sut.GetCurrentCompanyAsync();
            // Assert
            Assert.Equal(companyId, company!.Id);
        }

        [Fact]
        public async void GetCurrentCompany_ShouldReturnNull_WhenCurrentCompanyIsNull()
        {
            // Arrange
            // No setup necessary - CurrentCompany is null on instantiation
            // Act
            var company = await _sut.GetCurrentCompanyAsync();

            // Assert
            Assert.Null(company);
        }
    }
}
