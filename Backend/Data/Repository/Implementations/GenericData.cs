using Data.Repository.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations
{
    public class GenericData<T> : General<T>, IGenericData<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly ILogger<T> _logger;

        public GenericData(AppDbContext context, ILogger<T> logger)
            : base(context, logger) 
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>()
                            .Where(p => EF.Property<bool>(p, "Active") == true)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al traer los datos de la entidad con id {id}");
                throw;
            }
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo insertar los datos de la entidad {entity}");
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudo actualizar la entidad {entity}");
                throw;
            }
        }

        public virtual async Task<bool> DeletePersistenceAsync(int id)
        {
            try
            {
                var Delete = await GetByIdAsync(id);
                if (Delete == null) return false;

                _context.Set<T>().Remove(Delete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $" Error al eiminar de la entidad con id {id}");
                return false;
            }
        }

        public virtual async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                    return false;

                var property = entity.GetType().GetProperty("Active");
                if (property != null)
                {
                    property.SetValue(entity, false);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error al realizar la eliminación logica con LINQ {ex.Message}");
                return false;
            }
        }
    }
}