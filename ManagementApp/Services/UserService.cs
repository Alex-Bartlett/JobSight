﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserCompanyRepository _userCompanyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        private User? CurrentUser;

        public UserService(IUserCompanyRepository userCompanyRepository, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ICompanyRepository companyService, ILogger<UserService> logger)
        {
            _userCompanyRepository = userCompanyRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _companyRepository = companyService;
            _logger = logger;
        }

        /// <summary>
        /// Updates the current company for both the user and the company service.
        /// </summary>
        /// <param name="companyId">New company id</param>
        /// <returns>New company if successful, null if unsuccessful</returns>
        public async Task<User?> ChangeCurrentCompanyAsync(int companyId)
        {
            if (CurrentUser is null)
            {
                _logger.LogError("Could not change company because current user is not set.", [CurrentUser, companyId]);
                return null;
            }
            if (!await CompanyExists(companyId))
            {
                _logger.LogError("Could not change company because company does not exist.", [CurrentUser, companyId]);
                return null;
            }
            if (!await UserBelongsToCompany(CurrentUser, companyId))
            {
                _logger.LogError("Could not change company because user does not belong to company.", [CurrentUser, companyId]);
                return null;
            }

            CurrentUser.CurrentCompanyId = companyId;

            var result = await _userManager.UpdateAsync(CurrentUser);

            // Update current user regardless to update object properties/revert changes
            await UpdateCurrentUserAsync();

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to change company.", [result, CurrentUser, companyId]);
                return null;
            }

            return CurrentUser;
        }

        public async Task<int?> GetCurrentCompanyIdAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.CurrentCompanyId;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            if (CurrentUser == null)
            {
                // Try to update current user if null
                await UpdateCurrentUserAsync();
                if (CurrentUser == null)
                {
                    _logger.LogWarning("Current user could not be set.");
                }
            }

            return CurrentUser;
        }

        public async Task UpdateCurrentUserAsync()
        {
            var principal = _httpContextAccessor.HttpContext?.User;

            if (principal is null)
            {
                _logger.LogDebug("User not set in HttpContext");
                return;
            }
            var user = await _userManager.GetUserAsync(principal);
            if (user is null)
            {
                _logger.LogWarning("Matching user could not be found for ClaimPrincipal");
                return;
            }
            SetCurrentUser(user);
        }

        private void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        private async Task<bool> CompanyExists(int companyId)
        {
            // True if exists, false if not
            return await _companyRepository.GetByIdAsync(companyId) is not null;
        }

        private async Task<bool> UserBelongsToCompany(User user, int companyId)
        {
            return await _userCompanyRepository.UserBelongsToCompanyAsync(user.Id, companyId);
        }
    }
}
