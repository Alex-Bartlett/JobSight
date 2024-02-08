using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        public Customer? Customer { get; set; }
        [Required]
        public Company? Company { get; set; }
    }
}
