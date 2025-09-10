using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums.Models;

namespace Data.Repository.Implementations.System
{
    public class ZoneData : GenericData<Zone>, IZone
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public ZoneData(AppDbContext context, ILogger<Zone> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Zone>> GetAllAsync()
        {
            try
            {
                return await _context.Zone
                    .Include(fm => fm.Branch)
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

        public override async Task<Zone?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Zone
                    .Include(fm => fm.Branch)
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
        public override async Task<IEnumerable<Zone>> GetAllTotalAsync()
        {
            try
            {
                return await _context.Zone
                    .Include(fm => fm.Branch)
                    .Include(fm => fm.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener todos los datos");
                throw;
            }
        }

        public override async Task<IEnumerable<Zone>> GetAvailableZonesByUserAsync(int userId)
        {
            try
            {
                var now = DateTime.UtcNow;

                var zones = await _context.Operating
                    .Where(o => o.UserId == userId &&
                                o.OperationalGroup.DateStart <= now &&
                                (o.OperationalGroup.DateEnd == null || o.OperationalGroup.DateEnd >= now))
                    .SelectMany(o => o.OperationalGroup.User.Branch.Zones)
                    .Where(z => z.StateZone == StateZone.Available)
                    .Include(z => z.Branch)                  
                        .ThenInclude(b => b.Company)         
                    .ToListAsync();

                return zones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las zonas disponibles por usuario.");
                throw;
            }
        }



    }
}
