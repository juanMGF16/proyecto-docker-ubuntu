using Data.Repository.Interfaces.Specific.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.System
{
    /// <summary>
    /// Repositorio para gestión de verificadores
    /// </summary>
    public class CheckerData : GenericData<Checker>, ICheckerData
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public CheckerData(AppDbContext context, ILogger<Checker> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Inicia una transacción de base de datos
        /// </summary>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }


        /// <summary>
        /// Obtiene todos los verificadores activos con sus relaciones
        /// </summary>
        public override async Task<IEnumerable<Checker>> GetAllAsync()
        {
            try
            {
                return await _context.Checker
                    .Include(ch => ch.User)
                        .ThenInclude(u => u.Person)
                    .Include(ch => ch.Branch)
                    .Where(ch => ch.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un verificador por ID con sus relaciones
        /// </summary>
        /// <param name="id">ID del verificador</param>
        public override async Task<Checker?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Checker
                    .Include(ch => ch.User)
                        .ThenInclude(u => u.Person)
                    .Include(ch => ch.Branch)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un verificador por ID de usuario
        /// </summary>
        /// <param name="id">ID del usuario</param>
        public async Task<Checker?> GetByUserIdAsync(int id) { 
            try
            {
                return await _context.Checker
                    .Include(ch => ch.User)
                        .ThenInclude(u => u.Person)
                    .Include(ch => ch.Branch)
                    .FirstOrDefaultAsync(fm => fm.UserId == id && fm.Active);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por UserId");
                throw;
            }
        }
    }
}
