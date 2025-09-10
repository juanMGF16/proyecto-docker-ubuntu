namespace Data.Repository.Interfaces
{
    public interface IGenericData<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeletePersistenceAsync(int id);
        Task<bool> DeleteLogicalAsync(int id);
    }
}