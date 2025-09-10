using Entity.DTOs.SecurityModule;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IFormBusiness : IGenericBusiness<FormDTO, FormDTO>
    {
        // General
        Task<IEnumerable<FormDTO>> GetAllTotalFormsAsync();
    }
}