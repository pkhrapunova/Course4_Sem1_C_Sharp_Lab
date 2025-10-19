using Microsoft.EntityFrameworkCore;
using TechStore.Core.Data;
using TechStore.Core.Interfaces;
using TechStore.Core.Models;
using System.Threading.Tasks;

namespace TechStore.Core.Repositories
{
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		public UserRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}
	}
}