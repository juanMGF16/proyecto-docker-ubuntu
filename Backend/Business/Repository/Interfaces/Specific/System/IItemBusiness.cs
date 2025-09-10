using Entity.DTOs.System.Item;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IItemBusiness : IGenericBusiness<ItemConsultDTO, ItemDTO>
    {
        // General
        Task<IEnumerable<ItemConsultDTO>> GetAllTotalItemAsync();
        Task<IEnumerable<ItemConsultDTO>> GetAllItemsSpecificAsync(int zoneId);

    }
}
