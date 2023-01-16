using System;
namespace cat_cafe.Dto
{
	public class CustomerDto
	{
        public long Id { get; set; }
        public string? FullName { get; set; }
        public int Age { get; set; } = 0;
    }
}

