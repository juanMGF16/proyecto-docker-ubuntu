using Entity.DTOs.System;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IStateItemBusiness : IGenericBusiness<StateItemDTO, StateItemDTO>
    {
        // General
        Task<IEnumerable<StateItemDTO>> GetAllTotalStateItemAsync();
    }
}
