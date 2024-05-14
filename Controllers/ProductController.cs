using Fashion.Models;
using Fashion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
        {
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var response = await _productService.GetAllProducts();
			return Ok(response);
		}

		[HttpGet("{categoryName}")]
		public async Task<IActionResult> FilterProductsByCategory(string categoryName)
		{
			var response = await _productService.FilterProductsByCategory(categoryName);
			return Ok(response);
		}
		[HttpPost]
		public async Task<IActionResult> AddProduct(Product product)
		{
			var response = await _productService.AddProduct(product);
			return Ok(response);
		}
		[HttpPut]
		public async Task<IActionResult> UpdateProduct(Product product)
		{
			var response = await _productService.UpdateProduct(product);
			return Ok(response);
		}
		[HttpDelete("{productId}")]
		public async Task<IActionResult> DeleteProduct(int productId)
		{
			var response = await _productService.DeleteProduct(productId);
			return Ok(response);
		}
		[HttpGet("GetProduct/{productId}")]
		public async Task<IActionResult> GetProductById(int productId)
		{
			var response = await _productService.GetProductById(productId);
			return Ok(response);
		}
		[HttpPut("{productId}/{stockQuantity}")]
		public async Task<IActionResult> AddStockQuantityToProduct(int productId , int stockQuantity)
		{
			var response = await _productService.AddStockQuantityToProduct(productId , stockQuantity);
			return Ok(response);
		}
	}
}
