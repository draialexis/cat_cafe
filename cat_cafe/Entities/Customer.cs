using System;
using System.ComponentModel.DataAnnotations;

namespace cat_cafe.Entities
{
    public class Customer
    {
        
        public long Id { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public int Age { get; set; } = 0;
    }
}

