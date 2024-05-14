using Fashion.Models;

namespace Fashion.Services
{
	public interface IProductService
	{
		Task<ServiceResponse<List<Product>>> GetAllProducts();
		Task<ServiceResponse<Product>> GetProductById(int id);
		Task<ServiceResponse<List<Product>>> FilterProductsByCategory(string categoryName);
		Task<ServiceResponse<List<Product>>> AddProduct(Product product);
		Task<ServiceResponse<List<Product>>> UpdateProduct(Product product);
		Task<ServiceResponse<bool>> AddStockQuantityToProduct(int productId , int quantity);
		Task<ServiceResponse<bool>> DeleteProduct(int productId);
		
	}
}
