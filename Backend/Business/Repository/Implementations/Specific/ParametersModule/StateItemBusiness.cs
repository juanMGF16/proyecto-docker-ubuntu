using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System;
using Entity.Models.ParametersModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class StateItemBusiness :
        GenericBusinessSingleDTO<StateItem, StateItemDTO>,
        IStateItemBusiness
    {

        private readonly IGeneral<StateItem> _general;
        public StateItemBusiness(
            IDataFactoryGlobal factory,
            IGeneral<StateItem> general,
            IDeleteStrategyResolver<StateItem> deleteStrategyResolver,
            ILogger<StateItem> logger,
            IMapper mapper)
            : base(factory.CreateStateItemData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<StateItemDTO>> GetAllTotalStateItemAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<StateItemDTO>>(active);
        }

        protected override Task BeforeCreateMap(StateItemDTO dto, StateItem entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(StateItemDTO dto, StateItem entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(StateItemDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Name, dto.Name)))
                throw new ValidationException("Name", $"Ya existe un StateItem con el Name '{dto.Name}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(StateItemDTO dto, StateItem existingEntity)
        {
            if (!StringHelper.EqualsNormalized(existingEntity.Name, dto.Name))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Name, dto.Name)))
                    throw new ValidationException("Name", $"Ya existe un StateItem con el Name '{dto.Name}'.");
            }
        }
    }
}