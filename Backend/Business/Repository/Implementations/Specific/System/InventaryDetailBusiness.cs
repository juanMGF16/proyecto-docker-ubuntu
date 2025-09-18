using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Data.Repository.Interfaces.System;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.InventaryDetail;
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
    public class InventaryDetailBusiness :
        GenericBusinessDualDTO<InventaryDetail, InventaryDetailConsultDTO, InventaryDetailDTO>,
        IInventaryDetailBusiness
    {

        private readonly IGeneral<InventaryDetail> _general;
        public InventaryDetailBusiness(
            IDataFactoryGlobal factory,
            IGeneral<InventaryDetail> general,
            IDeleteStrategyResolver<InventaryDetail> deleteStrategyResolver,
            ILogger<InventaryDetail> logger,
            IMapper mapper)
            : base(factory.CreateInventaryDetailData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
        }

        //General 
        public async Task<IEnumerable<InventaryDetailConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<InventaryDetailConsultDTO>>(active);
        }

        protected override Task BeforeCreateMap(InventaryDetailDTO dto, InventaryDetail entity)
        {
            ValidationHelper.EnsureValidId(dto.StateItemId, "StateItemId");
            ValidationHelper.EnsureValidId(dto.InventaryId, "InventaryId");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(InventaryDetailDTO dto, InventaryDetail entity)
        {
            ValidationHelper.EnsureValidId(dto.StateItemId, "StateItemId");
            ValidationHelper.EnsureValidId(dto.InventaryId, "InventaryId");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(InventaryDetailDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => e.StateItemId == dto.StateItemId && e.InventaryId == dto.InventaryId))
                throw new ValidationException("Ya existe una relación con esos IDs.");
        }

        protected override async Task ValidateBeforeUpdateAsync(InventaryDetailDTO dto, InventaryDetail existingEntity)
        {
            if (dto.StateItemId != existingEntity.StateItemId || dto.InventaryId != existingEntity.InventaryId)
            {
                var existing = await _data.GetAllAsync();
                if (existing.Any(e => e.StateItemId == dto.StateItemId && e.InventaryId == dto.InventaryId && e.Id != dto.Id))
                    throw new ValidationException("Ya existe una relación con esos IDs.");
            }
        }
    }
}
