using System;
using cat_cafe.Entities;

namespace cat_cafe.Dto
{
	public class BarDto
	{
        public long Id { get; set; }
        public string? Name { get; set; }
        public List<CatDto> cats { get; set; } = new List<CatDto>();
    }
}

