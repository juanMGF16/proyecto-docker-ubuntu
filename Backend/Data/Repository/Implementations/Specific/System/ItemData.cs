
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.System
{
    /// <summary>
    /// Repositorio específico para la entidad Item.
    /// Extiende la funcionalidad genérica para incluir la generación automática de códigos QR
    /// al momento de crear un nuevo registro en base de datos.
    /// </summary>
    public class ItemData : GenericData<Item>, IItem
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        private readonly IQrCodeService _qrService;

        /// <summary>
        /// Constructor de la clase ItemData.
        /// Inyecta el contexto de base de datos, el logger y el servicio de generación de códigos QR.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        /// <param name="qrService">Servicio para generar y guardar códigos QR.</param>
        public ItemData(AppDbContext context, ILogger<Item> logger, IQrCodeService qrService)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _qrService = qrService;
        }
        public override async Task<IEnumerable<Item>> GetAllAsync()
        {
            try
            {
                return await _context.Item
                    .Include(fm => fm.CategoryItem)
                    .Include(fm => fm.StateItem)
                    .Include(fm => fm.Zone)
                    .Where(fm => fm.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<Item?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Item
                    .Include(fm => fm.CategoryItem)
                    .Include(fm => fm.StateItem)
                    .Include(fm => fm.Zone)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }

        // General
        public override async Task<IEnumerable<Item>> GetAllTotalAsync()
        {
            try
            {
                return await _context.Item
                    .Include(fm => fm.CategoryItem)
                    .Include(fm => fm.StateItem)
                    .Include(fm => fm.Zone)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener todos los datos");
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo Item en la base de datos y genera automáticamente su código QR.
        /// </summary>
        /// <param name="entity">Entidad Item a crear.</param>
        /// <returns>La entidad Item creada, con la ruta del código QR asignada.</returns>
        public override async Task<Item> CreateAsync(Item entity)
        {
            // Guardar primero para obtener el ID asignado por la base de datos
            await _context.Set<Item>().AddAsync(entity);
            await _context.SaveChangesAsync();

            // Contenido que se codificará en el QR (en este caso, el Id del Item)
            string content = $"Code:{entity.Code}";

            // Generar el QR y obtener la ruta del archivo guardado
            string qrPath = _qrService.GenerateAndSaveQrCode(content, "item_" + entity.Code);
            entity.QrPath = qrPath;

            // Actualizar el Item con la ruta del QR
            _context.Set<Item>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public override async Task<IEnumerable<Item>> GetAllItemsSpecific(int zonaId)
        {
            try
            {
                return await _context.Item
                    .Include(i => i.CategoryItem)
                    .Include(i => i.StateItem)
                    .Include(i => i.Zone)
                    .Where(i => i.ZoneId == zonaId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se pudieron obtener todos los datos");
                throw;
            }
        }


    }
}
