using cat_cafe.Entities;
using Microsoft.EntityFrameworkCore;

namespace cat_cafe.Repositories
{
    public class CatCafeContext : DbContext
    {
        public CatCafeContext(DbContextOptions<CatCafeContext> options)
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; } = null!;
        public DbSet<Bar> Bars { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;

    }
}
