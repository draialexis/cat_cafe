using System.ComponentModel.DataAnnotations;

namespace cat_cafe.Entities
{
    public class Cat
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public int Age { get; set; } = 0;
        public long? BarId { get; set; }
        public Bar? Bar { get; set; }
        public static string Meow { get; set; } = "meow";
    }
}
