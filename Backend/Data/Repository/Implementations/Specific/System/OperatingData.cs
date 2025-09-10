using Data.Repository.Interfaces.Specific.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.System
{
    public class OperatingData : GenericData<Operating>, IOperating
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public OperatingData(AppDbContext context, ILogger<Operating> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Operating>> GetAllAsync()
        {
            try
            {
                return await _context.Operating
                    .Include(fm => fm.User)
                    .Include(fm => fm.OperationalGroup)
                    .Where(fm => fm.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<Operating?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Operating
                    .Include(fm => fm.User)
                    .Include(fm => fm.OperationalGroup)
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
