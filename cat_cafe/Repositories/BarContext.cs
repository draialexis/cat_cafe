using cat_cafe.Entities;
using Microsoft.EntityFrameworkCore;
using System;
namespace cat_cafe.Repositories
{
	public class BarContext : DbContext
	{
        public BarContext(DbContextOptions<BarContext> options)
            : base(options)
        {
        }

        public DbSet<Bar> Bars { get; set; } = null!;

    }
}

