﻿using AutoMapper;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Specific.SecurityModule;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.SecurityModule.User;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.SecurityModule
{
    public class UserBusiness :
        GenericBusinessDualDTO<User, UserDTO, UserOptionsDTO>,
        IUserBusiness
    {

        private readonly IUserData _userData;
        private readonly IGeneral<User> _general;

        public UserBusiness(
            IDataFactoryGlobal factory,
            IGeneral<User> general,
            IDeleteStrategyResolver<User> deleteStrategyResolver,
            ILogger<User> logger,
            IMapper mapper)
            : base(factory.CreateUserData(), deleteStrategyResolver, logger, mapper)
        {
            _userData = factory.CreateUserData();
            _general = general;
        }

        // General 
        public async Task<IEnumerable<UserDTO>> GetAllTotalUsersAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(active);
        }

        // Specific
        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            var entity = await _userData.GetByUsernameAsync(username);
            return _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserCompanyCheckDTO> HasCompanyAsync(int userId)
        {
            ValidationHelper.EnsureValidId(userId, "UserId");

            try
            {
                return await _userData.HasCompanyAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en Business al obtener info de Company para el usuario {userId}");
                throw;
            }
        }

        public async Task<UserDTO> PartialUpdateAsync(UserPartialUpdateDTO dto)
        {
            ValidationHelper.EnsureValidId(dto.Id, "UserId");

            var user = await _userData.GetByIdAsync(dto.Id);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), dto.Id);

            var allUsers = await _userData.GetAllAsync();

            // --- Username ---
            if (!string.IsNullOrWhiteSpace(dto.Username) &&
                !StringHelper.EqualsNormalized(user.Username, dto.Username))
            {
                bool exists = allUsers.Any(u =>
                    u.Id != dto.Id &&
                    StringHelper.EqualsNormalized(u.Username, dto.Username));

                if (exists)
                    throw new ValidationException("Username", $"El Username '{dto.Username}' ya está en uso.");

                user.Username = dto.Username;
            }

            // --- Person asociada ---
            if (user.Person != null)
            {
                if (!string.IsNullOrWhiteSpace(dto.Email) &&
                    !StringHelper.EqualsNormalized(user.Person.Email, dto.Email))
                {
                    // Validar duplicado de Email
                    bool emailExists = allUsers.Any(u =>
                        u.Person != null &&
                        u.Id != dto.Id &&
                        StringHelper.EqualsNormalized(u.Person.Email, dto.Email));

                    if (emailExists)
                        throw new ValidationException("Email", $"El Email '{dto.Email}' ya está en uso.");

                    user.Person.Email = dto.Email;
                }

                if (!string.IsNullOrWhiteSpace(dto.Phone) &&
                    !StringHelper.EqualsNormalized(user.Person.Phone, dto.Phone))
                    user.Person.Phone = dto.Phone;
            }

            await _userData.UpdateAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDTO dto)
        {
            ValidationHelper.EnsureValidId(userId, "UserId");
            ValidationHelper.ThrowIfEmpty(dto.CurrentPassword, "Contraseña actual");
            ValidationHelper.ThrowIfEmpty(dto.NewPassword, "Nueva contraseña");

            var user = await _userData.GetByIdAsync(userId);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), userId);

            if (!PasswordHelper.Verify(user.Password, dto.CurrentPassword))
                throw new ValidationException("Password", "La contraseña actual es incorrecta");

            user.Password = PasswordHelper.Hash(dto.NewPassword);
            await _userData.UpdateAsync(user);
        }

        //Actions
        protected override Task BeforeCreateMap(UserOptionsDTO dto, User entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Username, "Username");
            ValidationHelper.ThrowIfEmpty(dto.Password, "Password");
            ValidationHelper.EnsureValidId(dto.PersonId, "PersonId");

            entity.Password = PasswordHelper.Hash(dto.Password);
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(UserOptionsDTO dto, User entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Username, "Username");
            ValidationHelper.EnsureValidId(dto.PersonId, "PersonId");

            if (!string.IsNullOrWhiteSpace(dto.Password))
                entity.Password = PasswordHelper.Hash(dto.Password);

            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(UserOptionsDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(f => StringHelper.EqualsNormalized(f.Username, dto.Username)))
                throw new ValidationException("Name", $"Ya existe un User con el Username '{dto.Username}'.");

            if (existing.Any(e => e.PersonId == dto.PersonId))
                throw new ValidationException("Combinación", "Ya existe una un User con el ID asociado de Person");
        }

        protected override async Task ValidateBeforeUpdateAsync(UserOptionsDTO dto, User existingEntity)
        {
            var others = await _data.GetAllAsync();

            if (!StringHelper.EqualsNormalized(existingEntity.Username, dto.Username))
            {

                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Username, dto.Username)))
                    throw new ValidationException("Name", $"Ya existe un User con el Username '{dto.Username}'.");
            }

            if (dto.PersonId != existingEntity.PersonId)
            {
                var existing = await _data.GetAllAsync();
                if (existing.Any(e => e.PersonId == dto.PersonId))
                    throw new ValidationException("Combinación", "Ya existe una un User con el ID asociado de Person");
            }
        }
    }
}