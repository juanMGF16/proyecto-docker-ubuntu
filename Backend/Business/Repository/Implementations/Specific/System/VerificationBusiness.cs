using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Verification;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class VerificationBusiness :
        GenericBusinessDualDTO<Verification, VerificationConsultDTO, VerificationDTO>,
        IVerificationBusiness
    {

        private readonly IGeneral<Verification> _general;
        public VerificationBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Verification> general,
            IDeleteStrategyResolver<Verification> deleteStrategyResolver,
            ILogger<Verification> logger,
            IMapper mapper)
            : base(factory.CreateVerificationData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<VerificationConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<VerificationConsultDTO>>(active);
        }

        protected override Task BeforeCreateMap(VerificationDTO dto, Verification entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Observations, "Observations");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(VerificationDTO dto, Verification entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Observations, "Observations");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(VerificationDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Observations, dto.Observations)))
                throw new ValidationException($"Ya existe la Observations '{dto.Observations}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(VerificationDTO dto, Verification existingEntity)
        {
            if (!StringHelper.EqualsNormalized(existingEntity.Observations, dto.Observations))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Observations, dto.Observations)))
                    throw new ValidationException($"Ya existe la Observations '{dto.Observations}'.");
            }
        }
    }
}
