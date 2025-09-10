using Entity.DTOs.System.Dashboard;

namespace Data.Repository.Interfaces.Specific.System.Others
{
    public interface IDashboardData
    {
        Task<DashboardDTO> GetDashboardAsync(DashboardFilterDTO filter);
        Task<Dictionary<string, int>> GetUsersByRoleAsync(int? companyId = null, int? branchId = null, int? zoneId = null);
    }
}
