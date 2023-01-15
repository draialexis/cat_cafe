using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cat_cafe.Entities;

namespace cat_cafe.Dto
{
    public class BarDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public List<Cat> cats { get; set; } = new List<Cat>();
    }
}
