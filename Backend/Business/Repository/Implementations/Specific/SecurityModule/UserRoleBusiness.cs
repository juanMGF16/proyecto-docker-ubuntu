using AutoMapper;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.SecurityModule.UserRole;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.SecurityModule
{
    public class UserRoleBusiness : 
        GenericBusinessDualDTO<UserRole, UserRoleDTO, UserRoleOptionsDTO>, 
        IUserRoleBusiness
    {

        private readonly IGeneral<UserRole> _general;

        public UserRoleBusiness(
            IDataFactoryGlobal factory, 
            IGeneral<UserRole> general,
            IDeleteStrategyResolver<UserRole> deleteStrategyResolver, 
            ILogger<UserRole> logger, 
            IMapper mapper)
            : base(factory.CreateUserRoleData(), deleteStrategyResolver, logger, mapper) 
        { 
            _general = general;
        }

        // General
        public async Task<IEnumerable<UserRoleDTO>> GetAllTotalUserRolesAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<UserRoleDTO>>(active);
        }
         
        //Specific


        //Actions
        protected override Task BeforeCreateMap(UserRoleOptionsDTO dto, UserRole entity)
        {
            ValidationHelper.EnsureValidId(dto.UserId, "UserId");
            ValidationHelper.EnsureValidId(dto.RoleId, "RolId");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(UserRoleOptionsDTO dto, UserRole entity)
        {
            ValidationHelper.EnsureValidId(dto.UserId, "UserId");
            ValidationHelper.EnsureValidId(dto.RoleId, "RolId");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(UserRoleOptionsDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => e.UserId == dto.UserId && e.RoleId == dto.RoleId))
                throw new ValidationException("Combinación", "Ya existe una relación User-Role con esos IDs.");
        }

        protected override async Task ValidateBeforeUpdateAsync(UserRoleOptionsDTO dto, UserRole existingEntity)
        {
            if (dto.UserId != existingEntity.UserId || dto.RoleId != existingEntity.RoleId)
            {
                var existing = await _data.GetAllAsync();
                if (existing.Any(e => e.UserId == dto.UserId && e.RoleId == dto.RoleId && e.Id != dto.Id))
                    throw new ValidationException("Combinación", "Ya existe una relación User-Role con esos IDs.");
            }
        }
    }
}