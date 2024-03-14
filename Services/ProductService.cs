using Fashion.Models;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Services
{
	public class ProductService : IProductService
	{
		private readonly AppDbContext _context;

		public ProductService(AppDbContext context)
        {
			_context = context;
		}

		public async Task<ServiceResponse<List<Product>>> AddProduct(Product product)
		{
			if (product != null)
			{
				await _context.Products.AddAsync(product);
				await _context.SaveChangesAsync();
				return new ServiceResponse<List<Product>> { Data = (await GetAllProducts()).Data};
			}
			
			return new ServiceResponse<List<Product>>() { Success = false };
		}

		public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if(product == null) 
				return new ServiceResponse<bool> {  Success = false  , Message = "Product not found!!"};

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return new ServiceResponse<bool> { Data = true };
		}

		public async Task<ServiceResponse<List<Product>>> FilterProductsByCategory(string categoryName)
		{
			var products = await _context.Products.Include(p => p.Category).Where(p => p.Category.Name.ToLower() == categoryName.ToLower()).ToListAsync();
			return new ServiceResponse<List<Product>> { Data = products };
		}

		public async Task<ServiceResponse<List<Product>>> GetAllProducts()
		{
			var products = await _context.Products.Include(p => p.Category).ToListAsync();
			return new ServiceResponse<List<Product>> { Data = products };
		}

		public async Task<ServiceResponse<List<Product>>> UpdateProduct(Product product)
		{
			var dbProduct = await _context.Products.FindAsync(product.Id);
			if(dbProduct == null)
				return new ServiceResponse<List<Product>> { Success = false , Message = "Product not found!!" };

			dbProduct.Title = product.Title;
			dbProduct.Description = product.Description;
			dbProduct.CategoryId = product.CategoryId;
			dbProduct.ImageUrl = product.ImageUrl;
			dbProduct.Price = product.Price;

			await _context.SaveChangesAsync();

			return new ServiceResponse<List<Product>> { Data = (await GetAllProducts()).Data };
		}
	}
}
