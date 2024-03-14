using Fashion.Models;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly AppDbContext _context;

		public CategoryService(AppDbContext context)
        {
			_context = context;
		}
        public async Task<ServiceResponse<List<Category>>> GetAllCategories()
		{
			var items = await _context.Categories.ToListAsync();
			return new ServiceResponse<List<Category>> { Data = items };
		}
	}
}
