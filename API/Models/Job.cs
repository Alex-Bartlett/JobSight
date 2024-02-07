using System.ComponentModel.DataAnnotations;

namespace API.Models
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
