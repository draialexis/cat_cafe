using System;
using cat_cafe.Entities;
using Microsoft.EntityFrameworkCore;
namespace cat_cafe.Repositories
{
	public class CustomerContext:DbContext
	{

        public readonly IConfiguration configuration;

        public CustomerContext(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder opts)
        {
            opts.UseSqlite(configuration.GetConnectionString(@"Data Source=CatCafe.db"));
        }


        /*public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        { }*/
        public DbSet<Customer> Customers { get; set; } = null!;
    }
}

