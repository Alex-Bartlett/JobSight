using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class JobNote
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        [Required]
        public int JobId { get; set; }
        public virtual Job? Job { get; set; }
    }
}
