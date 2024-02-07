using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class JobNote
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        [Required]
        public Job? Job { get; set; }
    }
}
