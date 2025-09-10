using Data.Repository.Interfaces.Specific.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.System
{
    public class VerificationData : GenericData<Verification>, IVerification
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public VerificationData(AppDbContext context, ILogger<Verification> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Verification>> GetAllAsync()
        {
            try
            {
                return await _context.Verification
                    .Include(fm => fm.Inventary)
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

        public override async Task<Verification?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Verification
                    .Include(fm => fm.Inventary)
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
