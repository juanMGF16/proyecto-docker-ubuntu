using Entity.DTOs.System.Company;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface ICompanyBusiness : IGenericBusiness<CompanyConsultDTO, CompanyDTO>
    {
        // General
        Task<IEnumerable<CompanyConsultDTO>> GetAllTotalAsync();
    }
}
