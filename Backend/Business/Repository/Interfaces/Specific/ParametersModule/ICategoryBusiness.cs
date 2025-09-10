using Entity.DTOs.ParametersModels.Category;

namespace Business.Repository.Interfaces.Specific.Parameters
{
    public interface ICategoryBusiness : IGenericBusiness<CategoryItemDTO, CategoryItemDTO>
    {
        // General
        Task<IEnumerable<CategoryItemDTO>> GetAllTotalCategoryAsync();
        Task<IEnumerable<CategoryItemListDTO>> GetAllItemsByZoneAsync(int zonaId);
    }
}
