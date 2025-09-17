using Entity.DTOs.System.Branch;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IBranchBusiness : IGenericBusiness<BranchConsultDTO, BranchDTO>
    {
        // General
        Task<IEnumerable<BranchConsultDTO>> GetAllTotalAsync();

        //Specific
        Task<IEnumerable<BranchSimpleDTO>> GetBranchesByCompanyAsync(int companyId);
        Task<BranchDetailsDTO?> GetBranchDetailsAsync(int branchId);
        Task<BranchInChargeDTO?> GetInChargeAsync(int branchId);
        Task<IEnumerable<BranchInChargeListDTO>> GetInChargesAsync(int companyId);
        Task<BranchConsultDTO> PartialUpdateAsync(BranchPartialUpdateDTO dto);
        Task<BranchConsultDTO?> GetBranchByInChargeAsync(int userId);

    }
}