using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Customer : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        [Required]
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
