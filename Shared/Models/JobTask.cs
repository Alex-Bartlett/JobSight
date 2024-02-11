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
        public Job? Job { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
