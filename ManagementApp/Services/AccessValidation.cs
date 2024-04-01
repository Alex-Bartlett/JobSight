using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace ManagementApp.Services
{
    public static class AccessValidation
    {
        /// <summary>
        /// Checks if the user's current company matches the entity's company. If not, throws an exception.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <param name="currentUser">The current user</param>
        /// <param name="logger">The service's logger</param>
        /// <exception cref="UnauthorizedAccessException">Thrown if the user is not allowed to access the job resource.</exception>
        public static void CheckForAuthorizationViolations(CompanySpecificEntity? entity, User currentUser, ILogger logger)
        {
            if (entity is null)
            {
                return;
            }
            // Company specific checks
            if (currentUser.CurrentCompanyId != entity.CompanyId)
            {
                logger.LogWarning("User attempted to access an unauthorized resource.", [entity, currentUser]);
                throw new UnauthorizedAccessException("User is not authorized to access this resource.");
            }
        }

        /// <summary>
        /// Checks if the user's current company matches the companyId. If not, throws an exception.
        /// </summary>
        /// <param name="companyId">The company id to check against</param>
        /// <param name="currentUser">The current user</param>
        /// <param name="logger">The service's logger</param>
        /// <exception cref="UnauthorizedAccessException">Thrown if the user is not allowed to access the job resource.</exception>
        public static void CheckForAuthorizationViolations(int companyId, User currentUser, ILogger logger)
        {
            // Company specific checks
            if (currentUser.CurrentCompanyId != companyId)
            {
                logger.LogWarning("User attempted to access an unauthorized resource.", [currentUser]);
                throw new UnauthorizedAccessException("User is not authorized to access this resource.");
            }
        }

        /// <summary>
        /// Validates the entity and checks if the user is authorized to access/modify it. CompanySpecificEntity entities are checked for authorization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity to validate</param>
        /// <param name="currentUser">The current user</param>
        /// <param name="logger">The service's logger</param>
        /// <returns>True if the entity is valid and the user is authorized to access/modify it, false otherwise.</returns>
        public static bool IsValid<T>(T entity, User? currentUser, ILogger logger)
        {
            if (currentUser is null)
            {
                logger.LogError($"User is null. Cannot validate {typeof(T).Name.ToLower()}.", entity);
                return false;
            }
            else if (currentUser.CurrentCompanyId is null)
            {
                logger.LogError($"Current company is null. Cannot validate {typeof(T).Name.ToLower()}.", [entity, currentUser]);
                return false;
            }

            if (entity is CompanySpecificEntity companySpecificEntity)
            {
                // Check if user is authorized to access/modify the customer
                try
                {
                    CheckForAuthorizationViolations(companySpecificEntity, currentUser, logger);
                }
                catch (UnauthorizedAccessException)
                {

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the job and checks if the user is authorized to access/modify it. CompanySpecificEntity entities are checked for authorization.
        /// </summary>
        /// <param name="job">The job to validate</param>
        /// <param name="currentUser">The current user</param>
        /// <param name="logger">The service's logger</param>
        /// <param name="customers">All current company's customers</param>
        /// <returns>True if the entity is valid and the user is authorized to access/modify it, false otherwise.</returns>
        public static bool IsValid(Job job, User currentUser, ILogger logger, IEnumerable<Customer>? customers)
        {
            if (!IsValid<Job>(job, currentUser, logger))
            {
                return false;
            }

            // Further validation for Job
            if (customers is null)
            {
                logger.LogError("Current company has no customers. Cannot validate job.", [job, currentUser]);
                return false;
            }

            // If customerId is not in the customer list of the company, throw an exception
            if (!customers.Any(c => c.Id == job.CustomerId))
            {
                logger.LogError("Customer does not belong to current company. Potential overposting attempt.", [job, currentUser]);
                return false;
            }

            return true;
        }
    }
}
