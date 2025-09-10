    using AutoMapper;
    using Data.Repository.Interfaces;
    using Data.Repository.Interfaces.Strategy;
    using Microsoft.Extensions.Logging;
    using Utilities.Enums;
    using Utilities.Exceptions;
    using Utilities.Helpers;

    namespace Business.Repository.Implementations
    {
        public abstract class GenericBusinessDualDTO<T, TReadDto, TWriteDto>
            where T : class
            where TWriteDto : class 
        {
            protected readonly IGenericData<T> _data;
            protected readonly IDeleteStrategyResolver<T> _deleteStrategyResolver;
            protected readonly ILogger<T> _logger;
            protected readonly IMapper _mapper;

            protected GenericBusinessDualDTO(
                IGenericData<T> data,
                IDeleteStrategyResolver<T> deleteStrategyResolver,
                ILogger<T> logger, 
                IMapper mapper)
            {
                _data = data;
                _deleteStrategyResolver = deleteStrategyResolver;
                _logger = logger;
                _mapper = mapper;
            }

            public virtual async Task<IEnumerable<TReadDto>> GetAllAsync()
            {
                var list = await _data.GetAllAsync();
                return _mapper.Map<IEnumerable<TReadDto>>(list);
            }

            public virtual async Task<TReadDto> GetByIdAsync(int id)
            {
                ValidationHelper.EnsureValidId(id, "ID");

                var entity = await _data.GetByIdAsync(id);
                if (entity == null)
                    throw new EntityNotFoundException(typeof(T).Name, id);

                return _mapper.Map<TReadDto>(entity);
            }

            public virtual async Task<TWriteDto> CreateAsync(TWriteDto dto)
            {
                ValidationHelper.ThrowIfNull(dto, nameof(dto));
                await ValidateBeforeCreateAsync(dto);

                var entity = _mapper.Map<T>(dto);
                await BeforeCreateMap(dto, entity);

                var created = await _data.CreateAsync(entity);
                return _mapper.Map<TWriteDto>(created);
            }

            public virtual async Task<TWriteDto> UpdateAsync(TWriteDto dto)
            {
                ValidationHelper.ThrowIfNull(dto, nameof(dto));

                var idProp = typeof(TWriteDto).GetProperty("Id")?.GetValue(dto);
                if (idProp == null || (int)idProp <= 0)
                    throw new ValidationException("Id", "El ID debe ser mayor que cero");

                var existing = await _data.GetByIdAsync((int)idProp);
                if (existing == null)
                    throw new EntityNotFoundException(typeof(T).Name, (int)idProp);

                await ValidateBeforeUpdateAsync(dto, existing);

                _mapper.Map(dto, existing);
                await BeforeUpdateMap(dto, existing);

                var updated = await _data.UpdateAsync(existing);
                return _mapper.Map<TWriteDto>(updated);
            }

            public virtual async Task<bool> DeleteAsync(int id, DeleteType strategyType)
            {
                ValidationHelper.EnsureValidId(id, "ID");

                var existing = await _data.GetByIdAsync(id);
                if (existing == null)
                    throw new EntityNotFoundException(typeof(T).Name, id);

                var strategy = _deleteStrategyResolver.Resolve(strategyType);
                return await strategy.DeleteAsync(id, _data);
            }

            // Hooks opcionales que puedes sobrescribir
            protected virtual Task BeforeCreateMap(TWriteDto dto, T entity) => Task.CompletedTask;
            protected virtual Task BeforeUpdateMap(TWriteDto dto, T entity) => Task.CompletedTask;

            // Métodos para validar
            protected virtual Task ValidateBeforeCreateAsync(TWriteDto dto) => Task.CompletedTask;
            protected virtual Task ValidateBeforeUpdateAsync(TWriteDto dto, T existingEntity) => Task.CompletedTask;
        }
    }