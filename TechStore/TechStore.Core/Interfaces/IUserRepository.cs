using System.Threading.Tasks;
using TechStore.Core.Models;
using TechStore.Core.Interfaces;

namespace TechStore.Core.Interfaces
{
	public interface UserRepository : IGenericRepository<User>
	{
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> GetUserByEmailAsync(string email);
	}
}