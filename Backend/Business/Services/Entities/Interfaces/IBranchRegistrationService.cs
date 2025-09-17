using Entity.DTOs.System.Branch.NestedCreation;

namespace Business.Services.Entities.Interfaces
{
    public interface IBranchRegistrationService
    {
        Task<BranchCreateResponseDTO> CreateBranchWithAdminAsync(BranchCreateRequestDTO request);
    }
}
