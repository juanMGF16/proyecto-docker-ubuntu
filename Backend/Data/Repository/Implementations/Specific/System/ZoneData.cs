using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        //public async Task<List<Zone>> GetZonesByUserAsync(int userId)
        //{
        //    var zones = await _context.Zone
        //        .Where(z => z.Active &&
        //            z.Inventories.Any(i =>
        //                i.OperatingGroup.Operatings.Any(o =>
        //                    o.UserId == userId && o.Active)))
        //        .ToListAsync();

        //    return zones;
        //}
        public override async Task<IEnumerable<Zone>> GetZonesByUserAsync(int userId)
        {
            try
            {
                return await _context.Zone
                    .Where(z => z.Active &&
                        z.Inventories.Any(i =>
                            i.OperatingGroup.Operatings.Any(o =>
                                o.UserId == userId && o.Active)))
                    .Include(z => z.Branch)   
                    .Include(z => z.User)   
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudieron obtener las zonas para el usuario {userId}");
                throw;
            }
        }

    }
}
