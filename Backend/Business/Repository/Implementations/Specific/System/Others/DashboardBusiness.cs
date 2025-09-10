using Business.Repository.Interfaces.Specific.System.Others;
using Data.Repository.Interfaces.Specific.System.Others;
using Entity.DTOs.System.Dashboard;

namespace Business.Repository.Implementations.Specific.System.Others
{
    public class DashboardBusiness : IDashboardBusiness
    {
        private readonly IDashboardData _dashboardData;
        public DashboardBusiness(IDashboardData dashboardData)
        {
            _dashboardData = dashboardData;
        }

        public async Task<DashboardDTO> GetDashboardAsync(DashboardFilterDTO filter)
        {
            // validaciones simples
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (filter.CompanyId <= 0) throw new ArgumentException("CompanyId inválido", nameof(filter.CompanyId));

            return await _dashboardData.GetDashboardAsync(filter);
        }

    }
}