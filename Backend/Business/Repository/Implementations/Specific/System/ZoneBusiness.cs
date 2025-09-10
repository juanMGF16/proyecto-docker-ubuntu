using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System.Zone;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class ZoneBusiness :
        GenericBusinessDualDTO<Zone, ZoneConsultDTO, ZoneDTO>,
        IZoneBusiness
    {

        private readonly IGeneral<Zone> _general;
        public ZoneBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Zone> general,
            IDeleteStrategyResolver<Zone> deleteStrategyResolver,
            ILogger<Zone> logger,
            IMapper mapper)
            : base(factory.CreateZoneData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<ZoneConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<ZoneConsultDTO>>(active);
        }
        

        protected override Task BeforeCreateMap(ZoneDTO dto, Zone entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(ZoneDTO dto, Zone entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(ZoneDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Name, dto.Name)))
                throw new ValidationException("Name", $"Ya existe un Branch con el Name '{dto.Name}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(ZoneDTO dto, Zone existingEntity)
        {
            if (!StringHelper.EqualsNormalized(existingEntity.Name, dto.Name))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Name, dto.Name)))
                    throw new ValidationException("Name", $"Ya existe un Brach con el Name '{dto.Name}'.");
            }
        }
    }
}