using Entity.Models.System;

namespace Data.Repository.Interfaces.General
{
    public interface IGetTotalGeneral<T> where T : class
    {
        Task<IEnumerable<T>> GetAllTotalAsync();
        Task<IEnumerable<T>> GetAllItemsSpecific(int id);

    }
}