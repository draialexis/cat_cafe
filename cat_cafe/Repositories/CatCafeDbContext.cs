using System;
using Microsoft.EntityFrameworkCore;
using cat_cafe.Entities;

namespace cat_cafe.Repositories
{
	public class CatCafeDbContext:DbContext
	{

		public DbSet<Customer> customers { get; set; }
		public DbSet<Bar> bars { get; set; }
		public DbSet<Cat> cats { get; set; }


		public CatCafeDbContext(DbContextOptions<CatCafeDbContext> opts)
			:base(opts)
		{
			Database.EnsureCreated();
		}

        public CatCafeDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=EFCatCafe.db").EnableSensitiveDataLogging();
        }

		
    }
}

