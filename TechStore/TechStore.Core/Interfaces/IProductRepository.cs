using System.Collections.Generic;
using System.Threading.Tasks;
using TechStore.Core.Models;
using TechStore.Core.Repositories;

namespace TechStore.Core.Interfaces
{
	public interface ProductRepository : IGenericRepository<Product>
	{
		Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
		Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
		Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize, string sortBy, string searchString, int? categoryId);
		Task<int> GetTotalProductsCountAsync(string searchString, int? categoryId);
	}
}