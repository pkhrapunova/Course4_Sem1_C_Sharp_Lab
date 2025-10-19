using Microsoft.EntityFrameworkCore;
using TechStore.Core.Data;
using TechStore.Core.Interfaces;
using TechStore.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechStore.Core.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public ProductRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
		{
			return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
		}

		public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
		{
			return await _context.Products
				.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize, string sortBy, string searchString, int? categoryId)
		{
			IQueryable<Product> query = _context.Products.Include(p => p.Category);

			if (!string.IsNullOrEmpty(searchString))
			{
				query = query.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
			}

			if (categoryId.HasValue && categoryId.Value > 0)
			{
				query = query.Where(p => p.CategoryId == categoryId.Value);
			}

			switch (sortBy?.ToLower())
			{
				case "name_asc":
					query = query.OrderBy(p => p.Name);
					break;
				case "name_desc":
					query = query.OrderByDescending(p => p.Name);
					break;
				case "price_asc":
					query = query.OrderBy(p => p.Price);
					break;
				case "price_desc":
					query = query.OrderByDescending(p => p.Price);
					break;
				default:
					query = query.OrderBy(p => p.Name);
					break;
			}

			return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
		}

		public async Task<int> GetTotalProductsCountAsync(string searchString, int? categoryId)
		{
			IQueryable<Product> query = _context.Products;

			if (!string.IsNullOrEmpty(searchString))
			{
				query = query.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
			}

			if (categoryId.HasValue && categoryId.Value > 0)
			{
				query = query.Where(p => p.CategoryId == categoryId.Value);
			}

			return await query.CountAsync();
		}
	}
}