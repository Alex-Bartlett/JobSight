using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        [Required]
        public int RoleId { get; set; }
        public UserRole? Role { get; set; }
    }
}
