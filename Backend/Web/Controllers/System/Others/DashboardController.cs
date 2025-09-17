using Business.Repository.Interfaces.Specific.System.Others;
using Entity.DTOs.System.Dashboard;
using Entity.DTOs.System.Dashboard.DashCompany;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.System.Others
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardBusiness _dashboardBusiness;
        public DashboardController(IDashboardBusiness dashboardBusiness)
        {
            _dashboardBusiness = dashboardBusiness;
        }

        [HttpGet]
        [Authorize(Roles = "SM_ACTION, ADMINISTRADOR")]
        public async Task<IActionResult> Get([FromQuery] int companyId, [FromQuery] int? branchId = null, [FromQuery] int? zoneId = null)
        {
            var filter = new DashboardFilterDTO
            {
                CompanyId = companyId,
                BranchId = branchId,
                ZoneId = zoneId
            };

            var result = await _dashboardBusiness.GetDashboardAsync(filter);
            return Ok(result);
        }

        [HttpGet("branch/{branchId:int}")]
        [Authorize(Roles = "SUBADMINISTRADOR, ADMINISTRADOR, SM_ACTION")]
        public async Task<IActionResult> GetByBranch(int branchId)
        {
            var result = await _dashboardBusiness.GetBranchDashboardAsync(branchId);
            return Ok(result);
        }

        [HttpGet("zone/{zoneId:int}/")]
        [Authorize(Roles = "ENCARGADO_ZONA, SUBADMINISTRADOR, ADMINISTRADOR, SM_ACTION")]
        public async Task<IActionResult> GetZoneDashboard(int zoneId)
        {
            var dashboard = await _dashboardBusiness.GetZoneDashboardAsync(zoneId);
            if (dashboard == null) return NotFound();

            return Ok(dashboard);
        }

    }
}
