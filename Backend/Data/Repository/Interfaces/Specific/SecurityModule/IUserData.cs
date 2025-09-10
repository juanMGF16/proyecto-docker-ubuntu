using Entity.Models.SecurityModule;

namespace Data.Repository.Interfaces.Specific.SecurityModule
{
    public interface IUserData : IGenericData<User>
    {
        Task<User?> GetByUsernameAsync(string username);

        //Specific
        Task<bool> HasCompanyAsync(int userId);
    }
}