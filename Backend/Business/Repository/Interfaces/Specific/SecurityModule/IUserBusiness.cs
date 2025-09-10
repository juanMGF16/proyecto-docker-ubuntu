using Entity.DTOs.SecurityModule.User;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IUserBusiness : IGenericBusiness<UserDTO, UserOptionsDTO>
    {
        // General
        Task<IEnumerable<UserDTO>> GetAllTotalUsersAsync();

        // Especific
        Task<UserDTO?> GetByUsernameAsync(string username);
        Task<UserCompanyCheckDTO> HasCompanyAsync(int userId);
        Task<UserDTO> PartialUpdateAsync(UserPartialUpdateDTO dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDTO dto);
    }
}