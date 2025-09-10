using Entity.DTOs.SecurityModule.RoleFormPermission;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IRoleFormPermissionBusiness : IGenericBusiness<RoleFormPermissionDTO, RoleFormPermissionOptionsDTO>
    {
        // General
        Task<IEnumerable<RoleFormPermissionDTO>> GetAllTotalRoleFormPermissionsAsync();
    }
}