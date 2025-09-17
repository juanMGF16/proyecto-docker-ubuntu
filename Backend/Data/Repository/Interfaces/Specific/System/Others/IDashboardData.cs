using Entity.DTOs.System.Dashboard.DashBranch;
using Entity.DTOs.System.Dashboard.DashCompany;
using Entity.Models.System;

namespace Data.Repository.Interfaces.Specific.System.Others
{
    public interface IDashboardData
    {
        Task<DashboardDTO> GetDashboardAsync(DashboardFilterDTO filter);
        Task<Dictionary<string, int>> GetUsersByRoleAsync(int? companyId = null, int? branchId = null, int? zoneId = null);
        Task<BranchDashboardDTO> GetBranchDashboardAsync(int branchId);
        Task<Zone?> GetZoneDashboardAsync(int zoneId);
        Task<List<OperatingGroup>> GetOperatingGroupsByUserIdAsync(int userId);
    }
}
