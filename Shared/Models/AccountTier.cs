using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class AccountTier
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
