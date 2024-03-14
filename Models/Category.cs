using System.Text.Json.Serialization;

namespace Fashion.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		[JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
