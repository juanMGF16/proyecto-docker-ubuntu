using Entity.DTOs.System.Dashboard;

namespace Business.Repository.Interfaces.Specific.System.Others
{
    public interface IDashboardBusiness
    {
        Task<DashboardDTO> GetDashboardAsync(DashboardFilterDTO filter);
    }
}
