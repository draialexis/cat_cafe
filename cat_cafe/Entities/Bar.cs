using System;
namespace cat_cafe.Entities
{
	public class Bar
	{
		
		
			public long Id { get; set; }
			public string? Name { get; set; }
			public List<Cat> cats { get; set; } = new List<Cat>();


		public void addCat(Cat c)
		{
			cats.Add(c);
		}
		public void removeCat(Cat c)
		{
			cats.Remove(c);
		}

	}
}

