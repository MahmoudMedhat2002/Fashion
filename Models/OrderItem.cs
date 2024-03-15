using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Fashion.Models
{
	public class OrderItem
	{
		public int Id { get; set; }
		[ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
		[ForeignKey("Order")]
		public int OrderId { get; set; }
		[JsonIgnore]
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
