using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System.OperatingGroup;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class OperatingGroupBusiness :
        GenericBusinessDualDTO<OperatingGroup, OperatingGroupConsultDTO, OperatingGroupDTO>,
        IOperatingGroupBusiness
    {

        private readonly IGeneral<OperatingGroup> _general;
        public OperatingGroupBusiness(
            IDataFactoryGlobal factory,
            IGeneral<OperatingGroup> general,
            IDeleteStrategyResolver<OperatingGroup> deleteStrategyResolver,
            ILogger<OperatingGroup> logger,
            IMapper mapper)
            : base(factory.CreateOperatingGroupData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<OperatingGroupConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<OperatingGroupConsultDTO>>(active);
        }

        //Specific


        //Actions
        protected override Task BeforeCreateMap(OperatingGroupDTO dto, OperatingGroup entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(OperatingGroupDTO dto, OperatingGroup entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(OperatingGroupDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Name, dto.Name)))
                throw new ValidationException($"Ya existe un Branch con el Name '{dto.Name}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(OperatingGroupDTO dto, OperatingGroup existingEntity)
        {
            if (!StringHelper.EqualsNormalized(existingEntity.Name, dto.Name))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Name, dto.Name)))
                    throw new ValidationException($"Ya existe un Brach con el Name '{dto.Name}'.");
            }
        }
    }
}
