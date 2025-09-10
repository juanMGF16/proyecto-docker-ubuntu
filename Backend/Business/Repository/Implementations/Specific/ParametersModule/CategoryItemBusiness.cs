using AutoMapper;
using Business.Repository.Interfaces.Specific.Parameters;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.DTOs.ParametersModels.Category;
using Entity.DTOs.System.Item;
using Entity.Models.ParametersModule;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.Parameters
{
    public class CategoryItemBusiness :
        GenericBusinessSingleDTO<CategoryItem, CategoryItemDTO>,
        ICategoryBusiness
    {

        private readonly IGeneral<CategoryItem> _general;
        private readonly IItemBusiness _itemBusiness;
        public CategoryItemBusiness(
            IDataFactoryGlobal factory,
            IGeneral<CategoryItem> general,
            IDeleteStrategyResolver<CategoryItem> deleteStrategyResolver,
            IItemBusiness itemBusiness,
            ILogger<CategoryItem> logger,
            IMapper mapper)
            : base(factory.CreateCategoryData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
            _itemBusiness = itemBusiness;
        }

        //General 
        public async Task<IEnumerable<CategoryItemDTO>> GetAllTotalCategoryAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<CategoryItemDTO>>(active);
        }

        //Specific
        public async Task<IEnumerable<CategoryItemListDTO>> GetAllItemsByZoneAsync(int zonaId)
        {
            // 1️Obtengo los items filtrados por zona desde la capa Data
            var items = await _itemBusiness.GetAllItemsSpecificAsync(zonaId);

            // 2️ Agrupo por categoría
            var grouped = items
                .GroupBy(i => new { i.CategoryItemId, i.CategoryName })
                .Select(g => new CategoryItemListDTO
                {
                    Id = g.Key.CategoryItemId,
                    Name = g.Key.CategoryName,
                    Items = g.Select(i => new ItemConsultCategoryDTO
                    {
                        Id = i.Id,
                        Name = i.Name,
                        StateItemId = i.StateItemId
                    })
                })
                .ToList();

            return grouped;
        }

        protected override Task BeforeCreateMap(CategoryItemDTO dto, CategoryItem entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(CategoryItemDTO dto, CategoryItem entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(CategoryItemDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Name, dto.Name)))
                throw new ValidationException("Name", $"Ya existe un category con el Name '{dto.Name}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(CategoryItemDTO dto, CategoryItem existingEntity)
        {
            if (!StringHelper.EqualsNormalized(existingEntity.Name, dto.Name))
            {
                var others = await _data.GetAllAsync();
                if (others.Any(e => e.Id != dto.Id && StringHelper.EqualsNormalized(e.Name, dto.Name)))
                    throw new ValidationException("Name", $"Ya existe un category con el Name '{dto.Name}'.");
            }
        }
    }
}