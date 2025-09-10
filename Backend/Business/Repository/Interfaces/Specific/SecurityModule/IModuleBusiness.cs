using Entity.DTOs.SecurityModule;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IModuleBusiness : IGenericBusiness<ModuleDTO, ModuleDTO>
    {
        // General
        Task<IEnumerable<ModuleDTO>> GetAllTotalModulesAsync();
    }
}