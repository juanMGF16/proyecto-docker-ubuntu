using Data.Repository.Interfaces.Specific.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.System
{
    public class OperatingGroupData : GenericData<OperatingGroup>, IOperatingGroup
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public OperatingGroupData(AppDbContext context, ILogger<OperatingGroup> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<OperatingGroup>> GetAllAsync()
        {
            try
            {
                return await _context.OperatingGroup
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

        public override async Task<OperatingGroup?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.OperatingGroup
                    .Include(fm => fm.User)
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
