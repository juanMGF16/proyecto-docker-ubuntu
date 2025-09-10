using Data.Repository.Interfaces.Specific.System.Others;
using Entity.Context;
using Entity.DTOs.System.Dashboard;
using Entity.Models.SecurityModule;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Implementations.Specific.System.Others
{
    public class DashboardData : IDashboardData
    {
        private readonly AppDbContext _context;

        public DashboardData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDTO> GetDashboardAsync(DashboardFilterDTO filter)
        {
            // Base queries (AsNoTracking porque son lecturas)
            var itemsQuery = _context.Item.AsNoTracking().Where(i => i.Active);
            var zonesQuery = _context.Zone.AsNoTracking().Where(z => z.Active);
            var branchesQuery = _context.Branch.AsNoTracking().Where(b => b.Active);

            // Aplicar scope según filtro (zone > branch > company)
            if (filter.ZoneId.HasValue)
            {
                var zoneId = filter.ZoneId.Value;
                itemsQuery = itemsQuery.Where(i => i.ZoneId == zoneId);
                zonesQuery = zonesQuery.Where(z => z.Id == zoneId);
                branchesQuery = branchesQuery.Where(b => b.Zones.Any(z => z.Id == zoneId));
            }
            else if (filter.BranchId.HasValue)
            {
                var branchId = filter.BranchId.Value;
                itemsQuery = itemsQuery.Where(i => i.Zone.BranchId == branchId);
                zonesQuery = zonesQuery.Where(z => z.BranchId == branchId);
                branchesQuery = branchesQuery.Where(b => b.Id == branchId);
            }
            else
            {
                var companyId = filter.CompanyId;
                itemsQuery = itemsQuery.Where(i => i.Zone.Branch.CompanyId == companyId);
                zonesQuery = zonesQuery.Where(z => z.Branch.CompanyId == companyId);
                branchesQuery = branchesQuery.Where(b => b.CompanyId == companyId);
            }

            var dto = new DashboardDTO
            {
                TotalItems = await itemsQuery.CountAsync(),
                TotalZones = await zonesQuery.CountAsync(),
                TotalBranches = await branchesQuery.CountAsync(),
                UsersByRole = await GetUsersByRoleAsync(filter.CompanyId, filter.BranchId, filter.ZoneId) 

            };

            // Items by category
            dto.ItemsByCategory = await itemsQuery
                .Where(i => i.CategoryItem != null)
                .GroupBy(i => i.CategoryItem.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category ?? "Sin categoría", x => x.Count);

            // Items by state
            dto.ItemsByState = await itemsQuery
                .Where(i => i.StateItem != null)
                .GroupBy(i => i.StateItem.Name)
                .Select(g => new { State = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.State ?? "Sin estado", x => x.Count);

            return dto;
        }

        public async Task<Dictionary<string, int>> GetUsersByRoleAsync(int? companyId = null, int? branchId = null, int? zoneId = null)
        {
            // Roles normales (UserRole)
            var userRolesQuery = _context.UserRole
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                    .ThenInclude(u => u.Company)
                .Include(ur => ur.User)
                    .ThenInclude(u => u.Branch)
                .Include(ur => ur.User)
                    .ThenInclude(u => u.Zone)
                .AsQueryable();

            // Filtros por alcance
            if (zoneId.HasValue)
            {
                userRolesQuery = userRolesQuery.Where(ur => ur.User.Zone != null && ur.User.Zone.Id == zoneId.Value);
            }
            else if (branchId.HasValue)
            {
                userRolesQuery = userRolesQuery.Where(ur => ur.User.Branch != null && ur.User.Branch.Id == branchId.Value);
            }
            else if (companyId.HasValue)
            {
                userRolesQuery = userRolesQuery.Where(ur =>
                    (ur.User.Branch != null && ur.User.Branch.CompanyId == companyId) ||
                    (ur.User.Zone != null && ur.User.Zone.Branch.CompanyId == companyId) ||
                    (ur.User.Company != null && ur.User.Company.Id == companyId)
                );
            }

            var roleCounts = await userRolesQuery
                .GroupBy(ur => ur.Role.Name)
                .Select(g => new { Role = g.Key, Count = g.Select(x => x.UserId).Distinct().Count() })
                .ToDictionaryAsync(x => x.Role, x => x.Count);

            // ---- OPERATIVOS ----
            var operativosQuery = _context.Operating
                .Include(o => o.OperationalGroup)
                    .ThenInclude(og => og.User)
                        .ThenInclude(u => u.Zone)
                .Include(o => o.OperationalGroup.User.Branch)
                .Include(o => o.OperationalGroup.User.Company)
                .AsQueryable();

            if (zoneId.HasValue)
            {
                operativosQuery = operativosQuery.Where(o => o.OperationalGroup.User.Zone != null && o.OperationalGroup.User.Zone.Id == zoneId.Value);
            }
            else if (branchId.HasValue)
            {
                operativosQuery = operativosQuery.Where(o => o.OperationalGroup.User.Branch != null && o.OperationalGroup.User.Branch.Id == branchId.Value);
            }
            else if (companyId.HasValue)
            {
                operativosQuery = operativosQuery.Where(o =>
                    (o.OperationalGroup.User.Branch != null && o.OperationalGroup.User.Branch.CompanyId == companyId) ||
                    (o.OperationalGroup.User.Zone != null && o.OperationalGroup.User.Zone.Branch.CompanyId == companyId) ||
                    (o.OperationalGroup.User.Company != null && o.OperationalGroup.User.Company.Id == companyId)
                );
            }

            var operativos = await operativosQuery.Select(o => o.UserId).Distinct().CountAsync();
            roleCounts["OPERATIVO"] = operativos;

            // ---- VERIFICADORES ----
            var verificadoresQuery = _context.Verification
                .Include(v => v.Inventary)
                    .ThenInclude(inv => inv.Zone)
                        .ThenInclude(z => z.Branch)
                .AsQueryable();

            if (zoneId.HasValue)
            {
                verificadoresQuery = verificadoresQuery.Where(v => v.Inventary.Zone.Id == zoneId.Value);
            }
            else if (branchId.HasValue)
            {
                verificadoresQuery = verificadoresQuery.Where(v => v.Inventary.Zone.Branch.Id == branchId.Value);
            }
            else if (companyId.HasValue)
            {
                verificadoresQuery = verificadoresQuery.Where(v => v.Inventary.Zone.Branch.CompanyId == companyId);
            }

            var verificadores = await verificadoresQuery.Select(v => v.UserId).Distinct().CountAsync();
            roleCounts["VERIFICADOR"] = verificadores;

            return roleCounts;
        }

    }
}

