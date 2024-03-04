using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    // Inherited by entities to add auditing
    public class AuditableEntity
    {
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public string? CreatedBy { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
        [Required]
        public string? UpdatedBy { get; set; }
    }
}