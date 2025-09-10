using Data.Repository.Interfaces.Specific.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Specific.System
{
    public class InventaryDetailData : GenericData<InventaryDetail>, IInventaryDetail
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public InventaryDetailData(AppDbContext context, ILogger<InventaryDetail> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<InventaryDetail>> GetAllAsync()
        {
            try
            {
                return await _context.InventaryDetail
                    .Include(fm => fm.Inventary)
                    .Include(fm => fm.Item)
                    .Include(fm => fm.StateItem)
                    .Where(fm => fm.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<InventaryDetail?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.InventaryDetail
                    .Include(fm => fm.Inventary)
                    .Include(fm => fm.Item)
                    .Include(fm => fm.StateItem)
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