using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Models
{
    public class UserCompany
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public User? User { get; set; }
        // This has to be nullable, a default value presents a security risk of setting a user to the wrong company.
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        // public Role Role { get; set; } // implement this later
    }
}
