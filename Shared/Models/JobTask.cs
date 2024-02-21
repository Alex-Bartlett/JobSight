using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class JobTask
    {
        [Required]
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        [Required]
        public int JobId { get; set; }
        public virtual Job? Job { get; set; }
        public virtual User? User { get; set; }
    }
}
