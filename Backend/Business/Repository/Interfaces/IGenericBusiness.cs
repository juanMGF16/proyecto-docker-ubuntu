using Utilities.Enums;

namespace Business.Repository.Interfaces
{
    public interface IGenericBusiness<TRead, TWrite>
    {
        Task<IEnumerable<TRead>> GetAllAsync();
        Task<TRead> GetByIdAsync(int id);
        Task<TWrite> CreateAsync(TWrite dto);
        Task<TWrite> UpdateAsync(TWrite dto);
        Task<bool> DeleteAsync(int id, DeleteType deleteType = DeleteType.Logical);
    }
}