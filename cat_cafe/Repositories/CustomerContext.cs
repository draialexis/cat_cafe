﻿using System;
using cat_cafe.Entities;
using Microsoft.EntityFrameworkCore;
namespace cat_cafe.Repositories
{
	public class CustomerContext:DbContext
	{
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        { }
        public DbSet<Customer> Customers { get; set; } = null!;
    }
}

