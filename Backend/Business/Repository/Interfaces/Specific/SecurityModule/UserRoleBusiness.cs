using Entity.DTOs.SecurityModule.UserRole;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IUserRoleBusiness : IGenericBusiness<UserRoleDTO, UserRoleOptionsDTO>
    {
        // General
        Task<IEnumerable<UserRoleDTO>> GetAllTotalUserRolesAsync();
    }
}