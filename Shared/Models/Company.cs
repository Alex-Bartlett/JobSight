﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? ContactNumber { get; set; }
        [Required]
        public AccountTier? AccountTier { get; set; }
    }
}