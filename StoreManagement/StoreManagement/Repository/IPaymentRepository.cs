using StoreManagement.Models;

namespace StoreManagement.Repository
{
	public interface IPaymentRepository : IRepository<int, Payment>
	{
		Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);
	}
}


