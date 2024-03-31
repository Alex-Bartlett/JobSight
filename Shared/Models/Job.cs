using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Job : CompanySpecificEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Reference { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? Description { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
