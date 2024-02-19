using ManagementApp.Services;
using Moq;
using Shared.Models;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ServiceTests
{
    public class CompanyServiceTests
    {
        private readonly CompanyService _sut;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();

        public CompanyServiceTests()
        {
            _sut = new CompanyService(_companyRepositoryMock.Object);
        }

        [Fact]
        public async void SetCompany_ShouldUpdateCompanyProperty_WhenCompanyExists()
        {
            // Arrange
            int companyId = 0;
            Company mockCompany = new()
            {
                Id = companyId,
            };
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(companyId)).ReturnsAsync(mockCompany);

            // Act
            await _sut.SetCompany(companyId);
            var actualId = _sut.Company?.Id;

            // Assert
            Assert.Equal(companyId, actualId);
        }

        [Fact]
        public async void SetCompany_ShouldReturnCompany_WhenCompanyExists()
        {
            // Arrange
            int companyId = 0;
            Company mockCompany = new()
            {
                Id = companyId,
            };
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(companyId)).ReturnsAsync(mockCompany);

            // Act
            var result = await _sut.SetCompany(companyId);

            // Assert
            Assert.Equal(companyId, result?.Id);
        }
    }
}
