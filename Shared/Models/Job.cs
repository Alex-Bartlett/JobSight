﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Job : AuditableEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Reference { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? Description { get; set; }
        [Required]
        public int? CustomerId { get; set; }
        [Required]
        public int? CompanyId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Company? Company { get; set; }
    }
}
