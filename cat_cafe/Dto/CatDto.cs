using System;
namespace cat_cafe.Dto
{
    public class CatDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; } = 0;
    }
}
