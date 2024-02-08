﻿using System.ComponentModel.DataAnnotations;

namespace JobSightLib.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
    }
}