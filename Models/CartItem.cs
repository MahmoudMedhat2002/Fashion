using System.ComponentModel.DataAnnotations.Schema;

namespace Fashion.Models
{
	public class CartItem
	{
        public int Id { get; set; }
        public string UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
