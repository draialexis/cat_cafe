//using System;
//using cat_cafe.Entities;
//using Microsoft.EntityFrameworkCore;
//namespace cat_cafe.Repositories
//{
//	public class CustomerContext:DbContext
//	{

//        public readonly IConfiguration configuration;

//        public string DbPath { get; }

//        public CustomerContext(IConfiguration _configuration)
//        {
//            configuration = _configuration;
//            var folder = Environment.SpecialFolder.LocalApplicationData;
//            var path = Environment.GetFolderPath(folder);
//            DbPath = System.IO.Path.Join(path, "CatCafe.db");
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder opts)
//        {
//            opts.UseSqlite($"Data Source=DbPath.db");
//        }


//        /*public CustomerContext(DbContextOptions<CustomerContext> options)
//            : base(options)
//        { }*/
//        public DbSet<Customer> Customers { get; set; } = null!;
//    }
//}

