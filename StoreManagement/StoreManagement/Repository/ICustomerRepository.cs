namespace StoreManagement.Repository
{
    public interface ICustomerRepository : IRepository<int, Models.Customer>
    {
        Task<Models.Customer?> GetCustomerByEmailAsync(string email);
        Task<Models.Customer?> GetCustomerByPhoneAsync(string phone);
        Task<IEnumerable<Models.Customer>> SearchCustomersByNameAsync(string name);
    }
}
