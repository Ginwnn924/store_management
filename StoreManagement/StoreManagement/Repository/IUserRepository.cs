using StoreManagement.Models;

namespace StoreManagement.Repository
{
	public interface IUserRepository : IRepository<int, User>
	{
		Task<User?> GetByUsernameAsync(string username);
		Task<IEnumerable<User>> SearchUsersByUsernameAsync(string username);
		Task<IEnumerable<User>> SearchUsersByFullNameAsync(string fullName);
		IQueryable<User> GetQueryable();
	}
}


