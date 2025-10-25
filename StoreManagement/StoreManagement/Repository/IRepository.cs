namespace StoreManagement.Repository
{
    public interface IRepository<TId, TModel> where TModel : class
    {
        Task<TModel> GetByIdAsync(TId id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> AddAsync(TModel entity);
        Task<TModel> UpdateAsync(TModel entity);
        Task<bool> DeleteAsync(TId id);
    }
}
