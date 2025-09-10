namespace Data.Repository.Interfaces.Strategy
{
    public interface IDeleteStrategy<T> where T : class
    {
        Task<bool> DeleteAsync(int id, IGenericData<T> data);
    }
}
