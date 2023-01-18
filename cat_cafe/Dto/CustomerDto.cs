using System;
using System.ComponentModel.DataAnnotations;

namespace cat_cafe.Dto
{
	public class CustomerDto
	{
        public long Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; } = 0;
    }
}

