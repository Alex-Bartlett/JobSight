using System.ComponentModel.DataAnnotations;

namespace JobSightLib.Models
{
    public class JobTaskImage
    {
        public int Id { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        [Required]
        public JobTask? JobTask { get; set; }
    }
}
