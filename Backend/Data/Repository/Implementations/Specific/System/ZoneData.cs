<<<<<<< HEAD
﻿using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.DTOs.System.Zone;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
>>>>>>> parent of 845d2803 (solucion de errores)

namespace Data.Repository.Implementations.System
{
    public class ZoneData : GenericData<Zone>, IZone
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public ZoneData(AppDbContext context, ILogger<Zone> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        //Contexto para Transacciones
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public override async Task<IEnumerable<Zone>> GetAllAsync()
        {
            try
            {
                return await _context.Zone
                    .Include(fm => fm.Branch)
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

        public override async Task<Zone?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Zone
                    .Include(fm => fm.Branch)
                    .Include(fm => fm.User)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }

        public override async Task<Zone> CreateAsync(Zone entity)
        {
            try
            {
                // Validar que el nombre no exista en la misma compañía
                var nameExists = await ZoneNameExistsAsync(entity.Name, entity.BranchId);
                if (nameExists)
                {
                    throw new ValidationException("Name", $"Ya existe una zona con el nombre {entity.Name} en esta sucursal");
                }

                await _context.Zone.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating branch");
                throw;
            }
        }

        // General
        public override async Task<IEnumerable<Zone>> GetAllTotalAsync()
        {
            try
            {
                return await _context.Zone
                    .Include(fm => fm.Branch)
                    .Include(fm => fm.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener todos los datos");
                throw;
            }
        }

<<<<<<< HEAD
        //Specific
        private async Task<bool> ZoneNameExistsAsync(string name, int branchId)
=======
        //public async Task<List<Zone>> GetZonesByUserAsync(int userId)
        //{
        //    var zones = await _context.Zone
        //        .Where(z => z.Active &&
        //            z.Inventories.Any(i =>
        //                i.OperatingGroup.Operatings.Any(o =>
        //                    o.UserId == userId && o.Active)))
        //        .ToListAsync();

        //    return zones;
        //}
        public override async Task<IEnumerable<Zone>> GetZonesByUserAsync(int userId)
>>>>>>> parent of 845d2803 (solucion de errores)
        {
            try
            {
                return await _context.Zone
<<<<<<< HEAD
                    .AnyAsync(b => b.Name.ToLower() == name.ToLower() &&
                                 b.BranchId == branchId &&
                                 b.Active);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if zone name exists: {Name}", name);
=======
                    .Where(z => z.Active &&
                        z.Inventories.Any(i =>
                            i.OperatingGroup.Operatings.Any(o =>
                                o.UserId == userId && o.Active)))
                    .Include(z => z.Branch)   
                    .Include(z => z.User)   
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"No se pudieron obtener las zonas para el usuario {userId}");
>>>>>>> parent of 845d2803 (solucion de errores)
                throw;
            }
        }

<<<<<<< HEAD
        public async Task<IEnumerable<Zone>> GetZonesByBranchAsync(int branchId)
        {
            try
            {
                return await _context.Zone
                    .Include(b => b.User)
                    .ThenInclude(u => u.Person)
                    .Where(b => b.BranchId == branchId && b.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo zones para branch: {BranchId}", branchId);
                throw;
            }
        }

        public async Task<Zone?> GetZoneDetailsAsync(int zoneId)
        {
            return await _context.Zone
                .Include(z => z.User)
                    .ThenInclude(u => u.Person)
                .Include(z => z.Items)
                    .ThenInclude(i => i.CategoryItem)
                .Include(z => z.Items)
                    .ThenInclude(i => i.StateItem)
                .Include(z => z.Inventories)
                .FirstOrDefaultAsync(z => z.Id == zoneId);
        }

        public async Task<IEnumerable<Zone>> GetInChargesAsync(int branchId)
        {
            try
            {
                return await _context.Zone
                    .Include(b => b.User)
                        .ThenInclude(u => u.Person)
                    .Where(b => b.BranchId == branchId && b.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting zone in-charges for branch: {BranchId}", branchId);
                throw;
            }
        }

        public async Task<Zone?> GetZoneByAreaManagerAsync(int userId)
        {
            try
            {
                return await _context.Zone
                    .Include(b => b.User)
                    .Include(b => b.Branch)
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.Active);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo zone para user: {UserId}", userId);
                throw;
            }
        }
=======
>>>>>>> parent of 845d2803 (solucion de errores)
    }
}
