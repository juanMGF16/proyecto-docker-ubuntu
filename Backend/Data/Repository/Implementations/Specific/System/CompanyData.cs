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
    }
}