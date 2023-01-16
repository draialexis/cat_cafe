using System;
namespace cat_cafe.Entities
{
    public class Customer
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public int Age { get; set; } = 0;
    }
}

