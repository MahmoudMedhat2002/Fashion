using Fashion.Models;

namespace Fashion.Services
{
	public interface IOrderService
	{
		Task<ServiceResponse<List<Order>>> GetOrders();
		Task<ServiceResponse<bool>> PlaceOrder();
	}
}
