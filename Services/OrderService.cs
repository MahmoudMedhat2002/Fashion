using Fashion.Models;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Services
{
	public class OrderService : IOrderService
	{
		private readonly AppDbContext _context;
		private readonly IAuthService _authService;

		public OrderService(AppDbContext context , IAuthService authService)
        {
			_context = context;
			_authService = authService;
		}
        public async Task<ServiceResponse<List<Order>>> GetOrders()
		{
			var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
				.Where(o => o.UserId == _authService.GetUserId()).ToListAsync();

			if(orders is null || orders.Count == 0)
			{
				return new ServiceResponse<List<Order>> { Message = "No Orders Found" };
			}

			return new ServiceResponse<List<Order>> { Data = orders };
		}

		public async Task<ServiceResponse<bool>> PlaceOrder()
		{
			string userId = _authService.GetUserId();

			var cartItems = await _context.CartItems.Include(ci => ci.Product).Where(ci => ci.UserId == userId).ToListAsync();

			var orderItems = new List<OrderItem>();

			decimal totalPrice = 0;

			foreach(var ci in cartItems)
			{
				orderItems.Add(new OrderItem
				{
					ProductId = ci.ProductId,
					Quantity = ci.Quantity
				});

				totalPrice += ci.Quantity * ci.Product.Price;
			}

			

			var order = new Order
			{
				UserId = userId,
				OrderItems = orderItems,
				TotalPrice = totalPrice
			};

			_context.CartItems.RemoveRange(cartItems);
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			return new ServiceResponse<bool> { Data = true };
		}
	}
}
