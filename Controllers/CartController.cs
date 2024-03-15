using Fashion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService)
        {
			_cartService = cartService;
		}
        [HttpGet]
		public async Task<IActionResult> GetCartItems()
		{
			var response = await _cartService.GetCartItems();
			return Ok(response);
		}
		[HttpPost("addtocart/{productId}")]
		public async Task<IActionResult> AddToCart(int productId)
		{
			var response = await _cartService.AddToCart(productId);
			return Ok(response);
		}
		[HttpPost("removefromcart/{productId}")]
		public async Task<IActionResult> RemoveFromCart(int productId)
		{
			var response = await _cartService.RemoveFromCart(productId);
			return Ok(response);
		}
	}
}
