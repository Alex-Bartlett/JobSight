using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class JobTask : AuditableEntity
    {
        [Required]
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        [Required]
        public int? JobId { get; set; }
        public virtual Job? Job { get; set; }
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public List<JobTaskImage>? Images { get; set; }
    }
}
