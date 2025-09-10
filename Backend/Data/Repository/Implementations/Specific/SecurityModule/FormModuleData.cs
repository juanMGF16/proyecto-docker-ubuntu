﻿using Data.Repository.Interfaces.Specific.SecurityModule;
using Entity.Context;
using Entity.Models.SecurityModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.SecurityModule
{
    public class FormModuleData : GenericData<FormModule>, IFormModuleData
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public FormModuleData(AppDbContext context, ILogger<FormModule> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<FormModule>> GetAllAsync()
        {
            try
            {
                return await _context.FormModule
                    .Include(fm => fm.Form)
                    .Include(fm => fm.Module)
                    .Where(fm => fm.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<FormModule?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.FormModule
                    .Include(fm => fm.Form)
                    .Include(fm => fm.Module)
                    .FirstOrDefaultAsync(fm => fm.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos por id");
                throw;
            }
        }

        // General
        public override async Task<IEnumerable<FormModule>> GetAllTotalAsync()
        {
            try
            {
                return await _context.FormModule
                    .Include(fm => fm.Form)
                    .Include(fm => fm.Module)
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