using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Models
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CartItem> CartItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Product>().HasData
			(
				new Product
				{
					Id = 1,
					Title = "Dress",
					Description = "This is Dress",
					ImageUrl = "https://i.pinimg.com/236x/d1/8e/f3/d18ef3e698b7c5822060aa9572bd5105.jpg",
					Price = 10.99m,
					CategoryId = 1
				}
				,
				new Product
				{
					Id = 2,
					Title = "T-Shirt",
					Description = "This is T-Shirt",
					ImageUrl = "https://i.pinimg.com/236x/28/9e/0d/289e0dc040aca1d87f0536e43969b15a.jpg",
					Price = 7.99m,
					CategoryId = 1
				}
				,
				new Product
				{
					Id = 3,
					Title = "Shoes",
					Description = "This is Shoes",
					ImageUrl = "https://i.pinimg.com/736x/c6/ef/fb/c6effbdce6a7da900fda401a52d15d96.jpg",
					Price = 8.99m,
					CategoryId = 2
				}
			);

			modelBuilder.Entity<Category>().HasData
			(
				new Category
				{
					Id = 1,
					Name = "Clothes"
				},
				new Category
				{
					Id = 2,
					Name = "Shoes"
				}
			);
		}

	}
}
