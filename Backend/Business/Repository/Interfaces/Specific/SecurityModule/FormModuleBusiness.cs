using Entity.DTOs.SecurityModule.FormModule;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IFormModuleBusiness : IGenericBusiness<FormModuleDTO, FormModuleOptionsDTO>
    {
        // General
        Task<IEnumerable<FormModuleDTO>> GetAllTotalFormModulesAsync();
    }
}