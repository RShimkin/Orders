namespace OrdersApp.Models.Repositories
{
    public interface IRepository<T>
    {
        Task<int> AddAsync(T entity);

        Task<IEnumerable<int>> AddRangeAsync(IEnumerable<T> entities);

        Task<T?> GetAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<int> DeleteAsync(int id);

        Task<int> DeleteRangeAsync(IEnumerable<int> ids);

        Task<int> UpdateAsync(T entity);
    }
}
