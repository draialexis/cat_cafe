namespace cat_cafe.Dto
{
    public class BarDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public List<long> CatIds { get; set; } = new();
    }
}

