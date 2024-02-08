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
        public Company? Company { get; set; }
        [Required]
        public UserRole? Role { get; set; }
    }
}
