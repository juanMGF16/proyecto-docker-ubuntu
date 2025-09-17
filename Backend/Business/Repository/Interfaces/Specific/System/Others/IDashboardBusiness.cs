using Entity.DTOs.System.Dashboard.DashBranch;
using Entity.DTOs.System.Dashboard.DashCompany;
using Entity.DTOs.System.Dashboard.DashZone;

namespace Business.Repository.Interfaces.Specific.System.Others
{
    public interface IDashboardBusiness
    {
        Task<DashboardDTO> GetDashboardAsync(DashboardFilterDTO filter);
        Task<BranchDashboardDTO> GetBranchDashboardAsync(int branchId);
        Task<ZoneDashboardDTO?> GetZoneDashboardAsync(int zoneId);
    }
}
