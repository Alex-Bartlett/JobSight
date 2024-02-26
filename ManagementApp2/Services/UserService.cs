using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.Models;
using Shared.Repositories;

namespace ManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        private User? CurrentUser;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Updates the current company for both the user and the company service.
        /// </summary>
        /// <param name="companyId">New company id</param>
        /// <returns>New company if successful, null if currentUser is unset/has no valid company</returns>
        public async Task<Company?> ChangeCurrentCompany(int companyId)
        {
            if (CurrentUser is null)
            {
                _logger.LogError("Could not change company because current user is not set.");
                return null;
            }
            var updatedUser = await _userRepository.ChangeCurrentCompanyAsync(CurrentUser, companyId);

            if (updatedUser is not null)
            {
                // Circular dependency avoidance. This can be maybe fixed with an intermediary service that handles updating current values?, but shouldnt work due to the nature of Scoped dependencies??. 

                /*var company = await _companyService.UpdateCurrentCompanyAsync(updatedUser.CurrentCompanyId);

                if (company is null || updatedUser is null)
                {
                    _logger.LogError("Failed to change current company. Change was successful in user service, but failed in company service.", [updatedUser, company]);
                }
                else
                {*/
                // Update currentUser with updatedUser containing the new current company
                CurrentUser = updatedUser;
                //}
            }

            return CurrentUser.CurrentCompany;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            if (CurrentUser == null)
            {
                // Try to update current user if null
                await UpdateCurrentUser();
                if (CurrentUser == null)
                {
                    _logger.LogWarning("Current user could not be set.");
                }
            }

            return CurrentUser;
        }

        public async Task UpdateCurrentUser()
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
            CurrentUser = user;
        }
    }
}
