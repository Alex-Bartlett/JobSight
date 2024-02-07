using System.ComponentModel.DataAnnotations;

namespace JobSightLib.Models
{
    public class JobTask
    {
        [Required]
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        [Required]
        public Job? Job { get; set; }
        [Required]
        public User? User { get; set; }
    }
}
