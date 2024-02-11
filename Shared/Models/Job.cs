using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Company? Company { get; set; }
    }
}
