using AutoMapper;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.SecurityModule.RoleFormPermission;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.SecurityModule
{
    public class RoleFormPermissionBusiness :
        GenericBusinessDualDTO<RoleFormPermission, RoleFormPermissionDTO, RoleFormPermissionOptionsDTO>,
        IRoleFormPermissionBusiness
    {

        private readonly IGeneral<RoleFormPermission> _general;

        public RoleFormPermissionBusiness(
            IDataFactoryGlobal factory, 
            IGeneral<RoleFormPermission> general,
            IDeleteStrategyResolver<RoleFormPermission> deleteStrategyResolver, 
            ILogger<RoleFormPermission> logger, 
            IMapper mapper)
            : base(factory.CreateRoleFormPermissionData(), deleteStrategyResolver, logger, mapper) 
        { 
            _general = general;
        }

        // General 
        public async Task<IEnumerable<RoleFormPermissionDTO>> GetAllTotalRoleFormPermissionsAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<RoleFormPermissionDTO>>(active);
        }

        //Specific


        //Actions
        protected override Task BeforeCreateMap(RoleFormPermissionOptionsDTO dto, RoleFormPermission entity)
        {
            ValidationHelper.EnsureValidId(dto.RoleId, "RolId");
            ValidationHelper.EnsureValidId(dto.FormId, "FormId");
            ValidationHelper.EnsureValidId(dto.PermissionId, "PermissionId");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(RoleFormPermissionOptionsDTO dto, RoleFormPermission entity)
        {
            ValidationHelper.EnsureValidId(dto.FormId, "FormId");
            ValidationHelper.EnsureValidId(dto.RoleId, "RolId");
            ValidationHelper.EnsureValidId(dto.PermissionId, "PermissionId");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(RoleFormPermissionOptionsDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => e.RoleId == dto.RoleId && e.FormId == dto.FormId && e.PermissionId == dto.PermissionId))
                throw new ValidationException("Combinación", "Ya existe una relación Role-Form-Permission con esos IDs.");
        }

        protected override async Task ValidateBeforeUpdateAsync(RoleFormPermissionOptionsDTO dto, RoleFormPermission existingEntity)
        {
            if (dto.RoleId != existingEntity.RoleId || dto.FormId != existingEntity.FormId || dto.PermissionId != existingEntity.PermissionId)
            {
                var existing = await _data.GetAllAsync();
                if (existing.Any(e => e.RoleId == dto.RoleId && e.FormId == dto.FormId && e.PermissionId == dto.PermissionId && e.Id != dto.Id))
                    throw new ValidationException("Combinación", "Ya existe una relación Role-Form-Permission con esos IDs.");
            }
        }
    }
}