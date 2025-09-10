using Entity.DTOs.SecurityModule;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IRoleBusiness : IGenericBusiness<RoleDTO, RoleDTO>
    {
        // General
        Task<IEnumerable<RoleDTO>> GetAllTotalRolesAsync();
    }
}