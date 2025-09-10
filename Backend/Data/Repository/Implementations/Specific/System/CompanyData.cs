using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.System
{
    public class CompanyData : GenericData<Company>, ICompany
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public CompanyData(AppDbContext context, ILogger<Company> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                return await _context.Company
                    .Include(fm => fm.User)
                    .Where(fm => fm.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<Company?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Company
                    .Include(fm => fm.User)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }

        // General
        public override async Task<IEnumerable<Company>> GetAllTotalAsync()
        {
            try
            {
                return await _context.Company
                    .Include(fm => fm.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener todos los datos");
                throw;
            }
        }

        //Specific
        public async Task<bool> KillCompanyAsync(int companyId)
        {
            try
            {
                var company = await _context.Company
                    .Include(c => c.Branches)
                        .ThenInclude(b => b.Zones)
                            .ThenInclude(z => z.Items)
                    .Include(c => c.Branches)
                        .ThenInclude(b => b.Zones)
                            .ThenInclude(z => z.Inventories)
                                .ThenInclude(i => i.InventaryDetails)
                    .Include(c => c.Branches)
                        .ThenInclude(b => b.Zones)
                            .ThenInclude(z => z.Inventories)
                                .ThenInclude(i => i.Verifications)
                    .FirstOrDefaultAsync(c => c.Id == companyId);

                if (company == null)
                    return false;

                // ------------------------
                // InventaryDetails y Verifications
                // ------------------------
                var inventaries = company.Branches
                    .SelectMany(b => b.Zones)
                    .SelectMany(z => z.Inventories)
                    .ToList();

                var inventaryDetails = inventaries
                    .SelectMany(i => i.InventaryDetails)
                    .ToList();

                var verifications = inventaries
                    .SelectMany(i => i.Verifications)
                    .ToList();


                _context.InventaryDetail.RemoveRange(inventaryDetails);
                _context.Verification.RemoveRange(verifications);

                // ------------------------
                // Inventaries y Items
                // ------------------------
                var zones = company.Branches
                    .SelectMany(b => b.Zones)
                    .ToList();

                var items = zones
                    .SelectMany(z => z.Items)
                    .ToList();

                var inventories = inventaries;

                _context.Item.RemoveRange(items);
                _context.Inventary.RemoveRange(inventories);

                // ------------------------
                // Zones y Branches
                // ------------------------
                var branches = company.Branches.ToList();

                _context.Zone.RemoveRange(zones);
                _context.Branch.RemoveRange(branches);

                // ------------------------
                // Company
                // ------------------------
                _context.Company.Remove(company);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar Company con id {companyId}");
                return false;
            }
        }

    }
}