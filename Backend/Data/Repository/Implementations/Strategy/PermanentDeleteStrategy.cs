using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Strategy;

namespace Data.Repository.Implementations.Strategy
{
    public class PermanentDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    {
        public async Task<bool> DeleteAsync(int id, IGenericData<T> data)
        {
            return await data.DeletePersistenceAsync(id);
        }
    }
}