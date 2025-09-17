using Business.Repository.Interfaces.Specific.System.Others;
using Data.Repository.Interfaces.Specific.System.Others;
using Entity.DTOs.System.Dashboard.DashBranch;
using Entity.DTOs.System.Dashboard.DashCompany;
using Entity.DTOs.System.Dashboard.DashZone;

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

        public async Task<BranchDashboardDTO> GetBranchDashboardAsync(int branchId)
        {
            if (branchId <= 0) throw new ArgumentException("BranchId inválido", nameof(branchId));
            return await _dashboardData.GetBranchDashboardAsync(branchId);
        }

        public async Task<ZoneDashboardDTO?> GetZoneDashboardAsync(int zoneId)
        {
            // Usar la capa de datos
            var zone = await _dashboardData.GetZoneDashboardAsync(zoneId);
            if (zone == null) return null;

            // -----------------------------
            // 1. Información general de la zona
            // -----------------------------
            var zoneInfo = new ZoneInfoDTO
            {
                ZoneId = zone.Id,
                ZoneName = zone.Name,
                State = zone.StateZone.ToString(),
                TotalItems = zone.Items.Count,
                InventoriesThisMonth = zone.Inventories.Count(i =>
                    i.Date.Month == DateTime.Now.Month &&
                    i.Date.Year == DateTime.Now.Year),
                LastInventoryDate = zone.Inventories
                    .OrderByDescending(i => i.Date)
                    .FirstOrDefault()?.Date,
                ZoneManagerName = $"{zone.User.Person.Name} {zone.User.Person.LastName}"
            };

            // -----------------------------
            // 2. Estado de ítems de la zona
            // -----------------------------
            var itemStatus = zone.Items
                .GroupBy(i => i.StateItem.Name)
                .Select(g => new ItemStatusDTO
                {
                    State = g.Key,
                    Count = g.Count()
                }).ToList();

            // -----------------------------
            // 3. Último inventario comparativo
            // -----------------------------
            var lastInventory = zone.Inventories
                .OrderByDescending(i => i.Date)
                .FirstOrDefault();

            var inventoryComparison = new List<InventoryItemCompareDTO>();

            if (lastInventory != null)
            {
                var details = lastInventory.InventaryDetails.ToList();
                inventoryComparison = zone.Items.Select(item =>
                {
                    var detail = details.FirstOrDefault(d => d.ItemId == item.Id);
                    return new InventoryItemCompareDTO
                    {
                        ItemName = item.Name,
                        Category = item.CategoryItem.Name,
                        ExpectedState = item.StateItem.Name,
                        FoundState = detail?.StateItem.Name ?? "No encontrado",
                        OperatingGroupName = lastInventory.OperatingGroup.Name
                    };
                }).ToList();
            }

            // -----------------------------
            // 4. Grupos operativos relacionados
            // -----------------------------
            // Obtener grupos propios usando la capa de datos
            var ownGroups = await _dashboardData.GetOperatingGroupsByUserIdAsync(zone.UserId);

            // Grupos de inventarios ya vienen cargados con la zona
            var groupsFromInventories = zone.Inventories
                .Select(i => i.OperatingGroup)
                .Distinct()
                .ToList();

            var allGroups = ownGroups
                .Concat(groupsFromInventories)
                .GroupBy(g => g.Id)
                .Select(g => g.First())
                .ToList();

            var operatingGroups = allGroups.Select(g => new OperatingGroupDashboardDTO
            {
                GroupId = g.Id,
                GroupName = g.Name,
                ScheduledStartDate = g.DateStart,
                ScheduledEndDate = g.DateEnd,
                ZoneManagerName = g.UserId == zone.UserId
                    ? "Actual Encargado"
                    : $"{g.User.Person.Name} {g.User.Person.LastName}",
                Operatives = g.Operatings
                    .Select(o => $"{o.User.Person.Name} {o.User.Person.LastName}")
                    .ToList()
            }).ToList();

            return new ZoneDashboardDTO
            {
                ZoneInfo = zoneInfo,
                ItemsStatus = itemStatus,
                InventoryComparison = inventoryComparison,
                OperatingGroups = operatingGroups
            };
        }
    }
}