using Fashion.Models;

namespace Fashion.Services
{
	public interface IProductService
	{
		Task<ServiceResponse<List<Product>>> GetAllProducts();
		Task<ServiceResponse<List<Product>>> FilterProductsByCategory(string categoryName);
		Task<ServiceResponse<List<Product>>> AddProduct(Product product);
		Task<ServiceResponse<List<Product>>> UpdateProduct(Product product);
		Task<ServiceResponse<bool>> DeleteProduct(int productId);
		
	}
}
