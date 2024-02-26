using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Models
{
    public class UserCompanies
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public User? User { get; set; }
        // Not required because it will cause a deadlock on sign-up. This should be managed manually
        public int? CompanyId { get; set; } = null;
        public Company? Company { get; set; }
        // public Role Role { get; set; } // implement this later
    }
}
