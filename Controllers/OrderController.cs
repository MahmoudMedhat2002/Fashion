using Fashion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
        {
			_orderService = orderService;
		}
        [HttpGet]
		public async Task<IActionResult> GetOrders()
		{
			var response = await _orderService.GetOrders();
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> PlaceOrder()
		{
			var response = await _orderService.PlaceOrder();
			return Ok(response);
		}
	}
}
