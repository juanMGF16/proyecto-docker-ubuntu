using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using CloudinaryDotNet.Core;
using Data.Factory;
using Data.Repository.Implementations.System;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Data.Repository.Interfaces.System;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Zone;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Enums.Models;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class ZoneBusiness :
        GenericBusinessDualDTO<Zone, ZoneConsultDTO, ZoneDTO>,
        IZoneBusiness
    {

        private readonly IGeneral<Zone> _general;
        private readonly IZone _zoneData;
        public ZoneBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Zone> general,
            IZone zoneData,
            IDeleteStrategyResolver<Zone> deleteStrategyResolver,
            ILogger<Zone> logger,
            IMapper mapper)
            : base(factory.CreateZoneData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
            _zoneData = zoneData;
        }

        //General 
        public async Task<IEnumerable<ZoneConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<ZoneConsultDTO>>(active);
        }

        //Specific
        public async Task<IEnumerable<ZoneSimpleDTO>> GetZonesByBranchAsync(int branchId)
        {
            ValidationHelper.EnsureValidId(branchId, "Branch ID");
            var branches = await _zoneData.GetZonesByBranchAsync(branchId);
            return _mapper.Map<IEnumerable<ZoneSimpleDTO>>(branches);
        }

        public async Task<ZoneDetailsDTO?> GetZoneDetailsAsync(int zoneId)
        {
            var zone = await _zoneData.GetZoneDetailsAsync(zoneId);
            if (zone == null) return null;

            return _mapper.Map<ZoneDetailsDTO>(zone);
        }

        public async Task<IEnumerable<ZoneInChargeListDTO>> GetInChargesAsync(int branchId)
        {
            ValidationHelper.EnsureValidId(branchId, "Branch ID");
            var inCharges = await _zoneData.GetInChargesAsync(branchId);

            return _mapper.Map<IEnumerable<ZoneInChargeListDTO>>(inCharges);
        }

        public async Task<ZoneConsultDTO?> GetZoneByAreaManagerAsync(int userId)
        {
            ValidationHelper.EnsureValidId(userId, "User ID");

            var branch = await _zoneData.GetZoneByAreaManagerAsync(userId);

            if (branch == null)
                return null;

            return _mapper.Map<ZoneConsultDTO>(branch);
        }

        public async Task<ZoneConsultDTO> PartialUpdateAsync(ZonePartialUpdateDTO dto)
        {
            ValidationHelper.EnsureValidId(dto.Id, "ZoneId");

            var zone = await _data.GetByIdAsync(dto.Id);
            if (zone == null)
                throw new EntityNotFoundException(nameof(Zone), dto.Id);

            var allZones = await _data.GetAllAsync();

            // --- Name ---
            if (!string.IsNullOrWhiteSpace(dto.Name) &&
                !StringHelper.EqualsNormalized(zone.Name, dto.Name))
            {
                bool nameExists = allZones.Any(c =>
                c.Id != dto.Id &&
                    StringHelper.EqualsNormalized(c.Name, dto.Name));

                if (nameExists)
                    throw new ValidationException("Zone", $"El nombre '{dto.Name}' ya está en uso.");

                zone.Name = dto.Name;
            }

            zone.Description = dto.Description;


            await _data.UpdateAsync(zone);
            return _mapper.Map<ZoneConsultDTO>(zone);
        }

        //Actions
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