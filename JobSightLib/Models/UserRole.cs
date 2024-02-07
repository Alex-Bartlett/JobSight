using System.ComponentModel.DataAnnotations;

namespace JobSightLib.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
