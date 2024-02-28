using ManagementApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Models;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ManagementApp.ServiceTests
{
    public class IdentityResultMock : IdentityResult
    {
        // Needed to mock userManager updates.
        // https://stackoverflow.com/a/38982103/22966636
        public IdentityResultMock(bool succeeded = false)
        {
            this.Succeeded = succeeded;
        }
    }

    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly Mock<IUserCompanyRepository> _userCompanyRepositoryMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new();
        private readonly Mock<ILogger<UserService>> _loggerMock = new();

        public UserServiceTests()
        {
            _userManagerMock = GenerateUserManagerMock();
            _sut = new UserService(_userCompanyRepositoryMock.Object, _httpContextAccessorMock.Object, _userManagerMock.Object, _companyRepositoryMock.Object, _loggerMock.Object);
        }
        private Mock<UserManager<User>> GenerateUserManagerMock()
        {
            // https://stackoverflow.com/a/52562694/22966636
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            return userManager;
        }

        [Fact]
        public async void ChangeCurrentCompanyAsync_ShouldUpdateCurrentCompany_WhenCompanyExists()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;
            string userId = "0";
            ClaimsPrincipal claimsPrincipal = new();

            User mockUser = new()
            {
                Id = userId,
                CurrentCompanyId = oldCompanyId,
            };
            Company mockCompany = new()
            {
                Id = newCompanyId,
            };
            _httpContextAccessorMock.SetupGet(x => x.HttpContext!.User).Returns(claimsPrincipal);
            _userManagerMock.Setup(x => x.GetUserAsync(claimsPrincipal)).ReturnsAsync(mockUser);
            _userCompanyRepositoryMock.Setup(x => x.UserBelongsToCompanyAsync(userId, newCompanyId)).ReturnsAsync(true);
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(mockCompany);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            // Act
            await _sut.UpdateCurrentUserAsync();
            await _sut.ChangeCurrentCompanyAsync(newCompanyId);
            var actualCompanyId = await _sut.GetCurrentCompanyIdAsync();

            // Assert
            Assert.Equal(newCompanyId, actualCompanyId);
        }

        [Fact]
        public async void ChangeCurrentCompanyAsync_ShouldNotUpdateCurrentCompany_WhenNewCompanyDoesNotExist()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;
            string userId = "0";
            ClaimsPrincipal claimsPrincipal = new();

            User mockUser = new()
            {
                Id = userId,
                CurrentCompanyId = oldCompanyId,
            };
            Company mockCompany = new()
            {
                Id = newCompanyId,
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext!.User).Returns(claimsPrincipal);
            _userManagerMock.Setup(x => x.GetUserAsync(claimsPrincipal)).ReturnsAsync(mockUser);
            _userCompanyRepositoryMock.Setup(x => x.UserBelongsToCompanyAsync(userId, newCompanyId)).ReturnsAsync(true);
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(() => null);

            // Act
            await _sut.ChangeCurrentCompanyAsync(newCompanyId);
            var actualCompanyId = await _sut.GetCurrentCompanyIdAsync();

            // Assert
            Assert.Equal(oldCompanyId, actualCompanyId);
        }

        [Fact]
        public async void ChangeCurrentCompanyAsync_ShouldReturnNull_WhenNewCompanyDoesNotExist()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;
            string userId = "0";
            ClaimsPrincipal claimsPrincipal = new();

            User mockUser = new()
            {
                Id = userId,
                CurrentCompanyId = oldCompanyId,
            };
            Company mockCompany = new()
            {
                Id = newCompanyId,
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext!.User).Returns(claimsPrincipal);
            _userManagerMock.Setup(x => x.GetUserAsync(claimsPrincipal)).ReturnsAsync(mockUser);
            _userCompanyRepositoryMock.Setup(x => x.UserBelongsToCompanyAsync(userId, newCompanyId)).ReturnsAsync(true);
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(() => null);

            // Act
            var result = await _sut.ChangeCurrentCompanyAsync(newCompanyId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void ChangeCurrentCompanyAsync_ShouldNotUpdateCurrentCompany_WhenUserDoesNotBelongToCompany()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;
            string userId = "0";
            ClaimsPrincipal claimsPrincipal = new();

            User mockUser = new()
            {
                Id = userId,
                CurrentCompanyId = oldCompanyId,
            };
            Company mockCompany = new()
            {
                Id = newCompanyId,
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext!.User).Returns(claimsPrincipal);
            _userManagerMock.Setup(x => x.GetUserAsync(claimsPrincipal)).ReturnsAsync(mockUser);
            _userCompanyRepositoryMock.Setup(x => x.UserBelongsToCompanyAsync(userId, newCompanyId)).ReturnsAsync(false);
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(mockCompany);

            // Act
            await _sut.ChangeCurrentCompanyAsync(newCompanyId);
            var actualCompanyId = await _sut.GetCurrentCompanyIdAsync();

            // Assert
            Assert.Equal(oldCompanyId, actualCompanyId);
        }

        [Fact]
        public async void ChangeCurrentCompanyAsync_ShouldReturnNull_WhenUserDoesNotBelongToCompany()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;
            string userId = "0";
            ClaimsPrincipal claimsPrincipal = new();

            User mockUser = new()
            {
                Id = userId,
                CurrentCompanyId = oldCompanyId,
            };
            Company mockCompany = new()
            {
                Id = newCompanyId,
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext!.User).Returns(claimsPrincipal);
            _userManagerMock.Setup(x => x.GetUserAsync(claimsPrincipal)).ReturnsAsync(mockUser);
            _userCompanyRepositoryMock.Setup(x => x.UserBelongsToCompanyAsync(userId, newCompanyId)).ReturnsAsync(false);
            _companyRepositoryMock.Setup(x => x.GetByIdAsync(newCompanyId)).ReturnsAsync(mockCompany);

            // Act
            var result = await _sut.ChangeCurrentCompanyAsync(newCompanyId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void ChangeCurrentCompanyAsync_ShouldReturnNull_WhenUserIsNull()
        {
            // Arrange
            int oldCompanyId = 0;
            int newCompanyId = 1;
            string userId = "0";
            ClaimsPrincipal claimsPrincipal = new();

            User mockUser = new()
            {
                Id = userId,
                CurrentCompanyId = oldCompanyId,
            };
            Company mockCompany = new()
            {
                Id = newCompanyId,
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(() => null);

            // Act
            var result = await _sut.ChangeCurrentCompanyAsync(newCompanyId);

            // Assert
            Assert.Null(result);
        }
    }
}
