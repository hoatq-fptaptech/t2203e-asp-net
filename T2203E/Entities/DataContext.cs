using System;
using Microsoft.EntityFrameworkCore;
namespace T2203E.Entities
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options): base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
	}
}

