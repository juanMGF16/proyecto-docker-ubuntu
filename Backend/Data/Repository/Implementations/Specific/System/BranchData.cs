
using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.SecurityModule;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.System
{
    public class BranchData : GenericData<Branch>, IBranch
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public BranchData(AppDbContext context, ILogger<Branch> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<Branch>> GetAllAsync()
        {
            try
            {
                return await _context.Branch
                    .Include(fm => fm.Company)
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

        public override async Task<Branch?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Branch
                    .Include(fm => fm.User)
                    .Include(fm => fm.Company)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }

        // General
        public override async Task<IEnumerable<Branch>> GetAllTotalAsync()
        {
            try
            {
                return await _context.Branch
                    .Include(fm => fm.Company)
                    .Include(fm => fm.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener todos los datos");
                throw;
            }
        }
    }
}


