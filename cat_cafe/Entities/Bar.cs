namespace cat_cafe.Entities
{
    public class Bar
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public List<Cat> Cats { get; set; } = new();

        public void AddCat(Cat c)
        {
            Cats.Add(c);
        }
        public void RemoveCat(Cat c)
        {
            Cats.Remove(c);
        }
    }
}

