using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        // Not required because it will cause a deadlock on sign-up. This should be managed manually
        public int? CurrentCompanyId { get; set; }
        public Company? CurrentCompany { get; set; }
    }

}
