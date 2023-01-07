using cat_cafe.Entities;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            String name = "Margot";
            int id = 1337;

            Cat cat = new()
            {
                Id = id,
                Name = name
            };

            Assert.Equal(name, cat.Name);
            Assert.Equal(id, cat.Id);
        }
    }
}