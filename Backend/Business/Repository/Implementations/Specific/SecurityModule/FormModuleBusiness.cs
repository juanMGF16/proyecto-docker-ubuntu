using AutoMapper;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.SecurityModule.FormModule;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.SecurityModule
{
    public class FormModuleBusiness :
        GenericBusinessDualDTO<FormModule, FormModuleDTO, FormModuleOptionsDTO>,
        IFormModuleBusiness
    {

        private readonly IGeneral<FormModule> _general;

        public FormModuleBusiness(
            IDataFactoryGlobal factory,
            IGeneral<FormModule> general,
            IDeleteStrategyResolver<FormModule> deleteStrategyResolver,
            ILogger<FormModule> logger, 
            IMapper mapper)
            : base(factory.CreateFormModuleData(), deleteStrategyResolver, logger, mapper) 
        { 
            _general = general;
        }

        // General 
        public async Task<IEnumerable<FormModuleDTO>> GetAllTotalFormModulesAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<FormModuleDTO>>(active);
        }

        //Specific


        //Actions
        protected override Task BeforeCreateMap(FormModuleOptionsDTO dto, FormModule entity)
        {
            ValidationHelper.EnsureValidId(dto.FormId, "FormId");
            ValidationHelper.EnsureValidId(dto.ModuleId, "RolId");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(FormModuleOptionsDTO dto, FormModule entity)
        {
            ValidationHelper.EnsureValidId(dto.FormId, "FormId");
            ValidationHelper.EnsureValidId(dto.ModuleId, "RolId");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(FormModuleOptionsDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => e.FormId == dto.FormId && e.ModuleId == dto.ModuleId))
                throw new ValidationException("Combinación", "Ya existe una relación Form-Module con esos IDs.");
        }

        protected override async Task ValidateBeforeUpdateAsync(FormModuleOptionsDTO dto, FormModule existingEntity)
        {
            if (dto.FormId != existingEntity.FormId || dto.ModuleId != existingEntity.ModuleId)
            {
                var existing = await _data.GetAllAsync();
                if (existing.Any(e => e.FormId == dto.FormId && e.ModuleId == dto.ModuleId && e.Id != dto.Id))
                    throw new ValidationException("Combinación", "Ya existe una relación Form-Module con esos IDs.");
            }
        }
    }
}