using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace T2203E.Entities
{
	[Table("Products")]
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		[Required]
		public double Price { get; set; }

		[StringLength(150)]
		public string Thumbnail { get; set; }
	}
}

