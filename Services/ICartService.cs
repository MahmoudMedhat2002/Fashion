using Fashion.Models;

namespace Fashion.Services
{
	public interface ICartService
	{
		Task<ServiceResponse<List<CartItem>>> GetCartItems();
		Task<ServiceResponse<List<CartItem>>> AddToCart(int productId);
		Task<ServiceResponse<List<CartItem>>> RemoveFromCart(int productId);
	}
}
