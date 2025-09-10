using System.Text.RegularExpressions;
using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.System.Company;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class CompanyBusiness :
        GenericBusinessDualDTO<Company, CompanyConsultDTO, CompanyDTO>,
        ICompanyBusiness
    {

        private readonly IGeneral<Company> _general;
        public CompanyBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Company> general,
            IDeleteStrategyResolver<Company> deleteStrategyResolver,
            ILogger<Company> logger,
            IMapper mapper)
            : base(factory.CreateCompanyData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<CompanyConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<CompanyConsultDTO>>(active);
        }

        protected override Task BeforeCreateMap(CompanyDTO dto, Company entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(CompanyDTO dto, Company entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(CompanyDTO dto)
        {

            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Name, dto.Name)))
                throw new ValidationException("Name", $"Ya existe un Company con el Name '{dto.Name}'.");

            if (existing.Any(e => StringHelper.EqualsNormalized(e.NIT, dto.NIT)))
                throw new ValidationException("NIT", $"Ya existe un Company con el NIT '{dto.NIT}'.");

            if (existing.Any(e => StringHelper.EqualsNormalized(e.Email, dto.Email)))
                throw new ValidationException("Email", $"Correo ya Registrado '{dto.Email}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(CompanyDTO dto, Company existingEntity)
        {
            // Validar formato del NIT si está siendo modificado
            if (!StringHelper.EqualsNormalized(existingEntity.NIT, dto.NIT))

            if (!StringHelper.EqualsNormalized(existingEntity.Name, dto.Name))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Name, dto.Name)))
                    throw new ValidationException("Name", $"Ya existe un Company con el Name '{dto.Name}'.");
            }

            if (!StringHelper.EqualsNormalized(existingEntity.NIT, dto.NIT))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.NIT, dto.NIT)))
                    throw new ValidationException("NIT", $"Ya existe un Company con el NIT '{dto.NIT}'.");
            }

            if (!StringHelper.EqualsNormalized(existingEntity.Email, dto.Email))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Email, dto.Email)))
                    throw new ValidationException("Email", $"Ya existe un Company con el Email '{dto.Name}'.");
            }
        }
    }
}