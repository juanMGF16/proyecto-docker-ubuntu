using Entity.DTOs.System.Branch;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IBranchBusiness : IGenericBusiness<BranchConsultDTO, BranchDTO>
    {
        // General
        Task<IEnumerable<BranchConsultDTO>> GetAllTotalAsync();
    }

}
