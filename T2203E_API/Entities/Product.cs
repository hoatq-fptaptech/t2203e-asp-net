using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace T2203E_API.Entities
{
	[Table("products")]
    [Index(nameof(name), IsUnique =true)]
    public class Product
	{
		[Key]
		public int id { get; set; }
		[Required]
        public string name { get; set; }
		[Required]
		public double price { get; set; }
		[Required]
		public string thumbnail { get; set; }
	}
}

