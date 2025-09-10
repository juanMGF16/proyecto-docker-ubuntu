using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System.Operating;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class OperatingBusiness :
        GenericBusinessDualDTO<Operating, OperatingConsultDTO, OperatingDTO>,
        IOperatingBusiness
    {

        private readonly IGeneral<Operating> _general;

        public OperatingBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Operating> general,
            IDeleteStrategyResolver<Operating> deleteStrategyResolver,
            ILogger<Operating> logger,
            IMapper mapper)
            : base(factory.CreateOperatingData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        // General 
        public async Task<IEnumerable<OperatingConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<OperatingConsultDTO>>(active);
        }

        // Specific
        

        //Actions
        protected override Task BeforeCreateMap(OperatingDTO dto, Operating entity)
        {
            ValidationHelper.EnsureValidId(dto.UserId, "userId");
            ValidationHelper.EnsureValidId(dto.OperationalGroupId, "OperationalGroupId");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(OperatingDTO dto, Operating entity)
        {
            ValidationHelper.EnsureValidId(dto.UserId, "UserId");
            ValidationHelper.EnsureValidId(dto.OperationalGroupId, "OperationalGroupId");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(OperatingDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => e.UserId == dto.UserId && e.OperationalGroupId == dto.OperationalGroupId))
                throw new ValidationException("Ya existe una relación con esos IDs.");
        }

        protected override async Task ValidateBeforeUpdateAsync(OperatingDTO dto, Operating existingEntity)
        {
            if (dto.UserId != existingEntity.UserId || dto.OperationalGroupId != existingEntity.OperationalGroupId)
            {
                var existing = await _data.GetAllAsync();
                if (existing.Any(e => e.UserId == dto.UserId && e.OperationalGroupId == dto.OperationalGroupId && e.Id != dto.Id))
                    throw new ValidationException("Ya existe una relación con esos IDs.");
            }
        }
    }

}
