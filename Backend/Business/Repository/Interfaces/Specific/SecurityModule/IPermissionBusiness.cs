using Entity.DTOs.SecurityModule;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IPermissionBusiness : IGenericBusiness<PermissionDTO, PermissionDTO>
    {
        // General
        Task<IEnumerable<PermissionDTO>> GetAllTotalPermissionsAsync();
    }
}