using Entity.DTOs.SecurityModule.User;
using Entity.Models.SecurityModule;

namespace Data.Repository.Interfaces.Specific.SecurityModule
{
    public interface IUserData : IGenericData<User>
    {
        Task<User?> GetByUsernameAsync(string username);

        //Specific
        Task<UserCompanyCheckDTO> HasCompanyAsync(int userId);
    }
}