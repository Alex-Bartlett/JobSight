using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? ContactNumber { get; set; }
        public int AccountTierId { get; set; }
        [Required]
        public virtual AccountTier? AccountTier { get; set; }
    }
}
