using Fashion.Models;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Services
{
	public class CartService : ICartService
	{
		private readonly AppDbContext _context;
		private readonly IAuthService _authService;

		public CartService(AppDbContext context , IAuthService authService)
        {
			_context = context;
			_authService = authService;
		}
        public async Task<ServiceResponse<List<CartItem>>> AddToCart(int productId)
		{
			var userId = _authService.GetUserId();
			var product = await _context.Products.FindAsync(productId);

			if(product == null)
			{
				return new ServiceResponse<List<CartItem>> { Success = false , Message = "Product not found !!!"};
			}

			var sameitem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

			if (sameitem == null)
			{
				var cartItem = new CartItem
				{
					ProductId = productId,
					UserId = _authService.GetUserId(),
				};

				await _context.CartItems.AddAsync(cartItem);
			}
			else
			{
				if (sameitem.Quantity + 1 > sameitem.Product.StockQuantity)
					return new ServiceResponse<List<CartItem>> { Success = false, Message = "There is not enough amout for this item" };

				sameitem.Quantity++;
			}

			await _context.SaveChangesAsync();

			return new ServiceResponse<List<CartItem>> { Data = (await GetCartItems()).Data };

		}

		public async Task<ServiceResponse<List<CartItem>>> GetCartItems()
		{
			var userId = _authService.GetUserId();
			var cartItems = await _context.CartItems.Include(ci => ci.Product)
				.Where(ci => ci.UserId == userId).ToListAsync();

			return new ServiceResponse<List<CartItem>> { Data = cartItems };
		}

		public async Task<ServiceResponse<int>> GetCartItemsCount()
		{
			var count = (await GetCartItems()).Data.Count;
			return new ServiceResponse<int> { Data = count };

		}

		public async Task<ServiceResponse<decimal>> GetTotalPrice()
		{
			var cartItems = (await GetCartItems()).Data;
			decimal totalPrice = 0;

			cartItems.ForEach(ci => totalPrice += (ci.Product.Price * ci.Quantity));

			return new ServiceResponse<decimal> { Data = totalPrice };
		}

		public async Task<ServiceResponse<List<CartItem>>> RemoveFromCart(int productId)
		{
			var userId = _authService.GetUserId();
			var product = await _context.Products.FindAsync(productId);

			if (product == null)
			{
				return new ServiceResponse<List<CartItem>> { Success = false, Message = "Product not found !!!" };
			}

			var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

			if (cartItem.Quantity > 1)
			{
				cartItem.Quantity--;
			}
			else
			{
				_context.CartItems.Remove(cartItem);
			}

			await _context.SaveChangesAsync();

			return new ServiceResponse<List<CartItem>> { Data = (await GetCartItems()).Data };
		}
	}
}
