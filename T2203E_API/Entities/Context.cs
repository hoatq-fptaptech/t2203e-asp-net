using System;
using Microsoft.EntityFrameworkCore;
namespace T2203E_API.Entities
{
	public class Context : DbContext
	{
		public Context(DbContextOptions options):base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}

