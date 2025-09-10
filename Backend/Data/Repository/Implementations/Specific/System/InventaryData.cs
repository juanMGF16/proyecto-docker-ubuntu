using Data.Repository.Interfaces.Specific.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.System
{
    public class InventaryData : GenericData<Inventary>, IInventary
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public InventaryData(AppDbContext context, ILogger<Inventary> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Inventary>> GetAllAsync()
        {
            try
            {
                return await _context.Inventary
                    .Include(fm => fm.OperatingGroup)
                    .Include(fm => fm.Zone)
                    .Where(fm => fm.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<Inventary?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Inventary
                    .Include(fm => fm.OperatingGroup)
                    .Include(fm => fm.Zone)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }
    }
}
