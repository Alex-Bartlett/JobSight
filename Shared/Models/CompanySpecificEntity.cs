using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    // All Company Specific entities are auditable
    public class CompanySpecificEntity : AuditableEntity
    {
        [Required]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}