using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace T2203E_API.Entities
{
	[Table("categories")]
	[Index(nameof(name),IsUnique =true)]
	public class Category
	{
		[Key]
		public int id { get; set; }
		[Required]
		public string name { get; set; }
	}
}

