using AutoMapper;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Specific.SecurityModule;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.SecurityModule.Person;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.SecurityModule
{
    public class PersonBusiness : 
        GenericBusinessSingleDTO<Person, PersonDTO>, 
        IPersonBusiness

    {
        private readonly IPersonData _personData;
        private readonly IGeneral<Person> _general;
        public PersonBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Person> general,
            IDeleteStrategyResolver<Person> deleteStrategyResolver, 
            ILogger<Person> logger, 
            IMapper mapper)
            : base(factory.CreatePersonData(), deleteStrategyResolver, logger, mapper) 
        {
            _personData = factory.CreatePersonData();
            _general = general;
        }

        //General 
        public async Task<IEnumerable<PersonDTO>> GetAllTotalPersonsAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<PersonDTO>>(active);
        }

        // Specific
        public async Task<IEnumerable<PersonAvailableDTO?>> GetPersonAvailableAsync()
        {
            var entities = await _personData.GetAvailablePersons();
            return _mapper.Map<IEnumerable<PersonAvailableDTO?>>(entities);

        }

        //Actions
        protected override Task BeforeCreateMap(PersonDTO dto, Person entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(PersonDTO dto, Person entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(PersonDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Email, dto.Email)))
                throw new ValidationException("Email", $"Correo ya Registrado '{dto.Email}'.");

            if (existing.Any(e => StringHelper.EqualsNormalized(e.DocumentNumber, dto.DocumentNumber)))
                throw new ValidationException("Document", $"Documento Ya Registrado '{dto.DocumentNumber}'.");

            if (existing.Any(e => StringHelper.EqualsNormalized(e.Phone, dto.Phone)))
                throw new ValidationException("Phone", $"Telefono ya Registrado '{dto.Phone}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(PersonDTO dto, Person existingEntity)
        {
            var documentNumberNormalized = StringHelper.EqualsNormalized(existingEntity.DocumentNumber, dto.DocumentNumber);
            var phoneNormalized = StringHelper.EqualsNormalized(existingEntity.Phone, dto.Phone);

            if (documentNumberNormalized || phoneNormalized)
            {
                var others = await _data.GetAllAsync();

                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.DocumentNumber, dto.DocumentNumber)))
                    throw new ValidationException("Document", $"Document Ya Registrado '{dto.DocumentNumber}'.");

                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Phone, dto.Phone)))
                    throw new ValidationException("Phone", $"Phone Ya Registrado '{dto.Phone}'.");
            }
        }

    }
}