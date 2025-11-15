using StoreManagement.Models;

namespace StoreManagement.Repository
{
	public interface ISupplierRepository : IRepository<int, Supplier>
	{
		Task<IEnumerable<Supplier>> SearchSuppliersByNameAsync(string name);
		Task<Supplier?> GetSupplierByEmailAsync(string email);
		Task<Supplier?> GetSupplierByPhoneAsync(string phone);
		IQueryable<Supplier> GetQueryable();
	}
}





