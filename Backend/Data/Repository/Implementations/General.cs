using Data.Repository.Interfaces.General;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations
{
    public class General<T> : IGeneral<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly ILogger<T> _logger;

        public General(AppDbContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<T>> GetAllTotalAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error al obtener todos los registros.");
                throw;
            }   
        }

        public virtual async Task<IEnumerable<T>> GetAllItemsSpecific(int zonaId)
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Error al obtener registros por zona.");
                throw;
            }
        }


    }
}