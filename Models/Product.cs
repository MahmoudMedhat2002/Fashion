using System.ComponentModel.DataAnnotations.Schema;

namespace Fashion.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string ImageUrl { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int StockQuantity { get; set; } = 1;
        public Category? Category { get; set; }

		[ForeignKey("Category")]
		public int CategoryId { get; set; }

	}
}
