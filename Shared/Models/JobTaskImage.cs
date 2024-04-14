using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class JobTaskImage
    {
        public int Id { get; set; }
        [Required]
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        [Required]
        public int JobTaskId { get; set; }
        public virtual JobTask? JobTask { get; set; }
    }
}
