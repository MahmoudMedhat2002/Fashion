using Fashion.Models;

namespace Fashion.Services
{
	public interface ICategoryService
	{
		public Task<ServiceResponse<List<Category>>> GetAllCategories();
	}
}
