using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class JobTaskImage
    {
        public int Id { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        [Required]
        public int JobTaskId { get; set; }
        public JobTask? JobTask { get; set; }
    }
}
