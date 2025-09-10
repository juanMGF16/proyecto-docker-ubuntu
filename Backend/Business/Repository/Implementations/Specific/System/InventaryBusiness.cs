using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System.Inventary;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class InventaryBusiness :
        GenericBusinessDualDTO<Inventary, InventaryConsultDTO, InventaryDTO>,
        IInventaryBusiness
    {

        private readonly IGeneral<Inventary> _general;
        public InventaryBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Inventary> general,
            IDeleteStrategyResolver<Inventary> deleteStrategyResolver,
            ILogger<Inventary> logger,
            IMapper mapper)
            : base(factory.CreateInventaryData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<InventaryConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<InventaryConsultDTO>>(active);
        }

        //Specific


        //Actions
        protected override Task BeforeCreateMap(InventaryDTO dto, Inventary entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Observations, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(InventaryDTO dto, Inventary entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Observations, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(InventaryDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Observations, dto.Observations)))
                throw new ValidationException($"Ya existe un Branch con el Name '{dto.Observations}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(InventaryDTO dto, Inventary existingEntity)
        {
            if (!StringHelper.EqualsNormalized(existingEntity.Observations, dto.Observations))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Observations, dto.Observations)))
                    throw new ValidationException($"Ya existe un Brach con el Name '{dto.Observations}'.");
            }
        }
    }
}
