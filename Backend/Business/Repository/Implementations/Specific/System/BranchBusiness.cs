using AutoMapper;
using Business.Repository.Interfaces.Specific.System;
using Data.Factory;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Data.Repository.Interfaces.System;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Company;
using Entity.Models.System;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Business.Repository.Implementations.Specific.System
{
    public class BranchBusiness :
        GenericBusinessDualDTO<Branch, BranchConsultDTO, BranchDTO>,
        IBranchBusiness
    {

        private readonly IGeneral<Branch> _general;
        private readonly IBranch _branchData;
        //private read
        public BranchBusiness(
            IDataFactoryGlobal factory,
            IGeneral<Branch> general,
            IBranch brancData,
            IDeleteStrategyResolver<Branch> deleteStrategyResolver,
            ILogger<Branch> logger,
            IMapper mapper)
            : base(factory.CreateBranchData(), deleteStrategyResolver, logger, mapper)
        {
            _general = general;
            _branchData = brancData;
        }

        //General 
        public async Task<IEnumerable<BranchConsultDTO>> GetAllTotalAsync()
        {
            var active = await _general.GetAllTotalAsync();
            return _mapper.Map<IEnumerable<BranchConsultDTO>>(active);
        }

<<<<<<< HEAD
        //Specific
        public async Task<IEnumerable<BranchSimpleDTO>> GetBranchesByCompanyAsync(int companyId)
        {
            ValidationHelper.EnsureValidId(companyId, "Company ID");
            var branches = await _branchData.GetBranchesByCompanyAsync(companyId);
            return _mapper.Map<IEnumerable<BranchSimpleDTO>>(branches);
        }

        public async Task<BranchDetailsDTO?> GetBranchDetailsAsync(int branchId)
        {
            ValidationHelper.EnsureValidId(branchId, "Branch ID");

            var branch = await _branchData.GetBranchWithZonesAndItemsAsync(branchId);

            if (branch == null)
                return null;

            return _mapper.Map<BranchDetailsDTO>(branch);
        }

        public async Task<BranchInChargeDTO?> GetInChargeAsync(int branchId)
        {
            ValidationHelper.EnsureValidId(branchId, "Branch ID");

            var branch = await _branchData.GetInChargeAsync(branchId);

            if (branch == null)
                return null;

            return _mapper.Map<BranchInChargeDTO>(branch);
        }

        public async Task<IEnumerable<BranchInChargeListDTO>> GetInChargesAsync(int companyId)
        {
            ValidationHelper.EnsureValidId(companyId, "Company ID");
            var inCharges = await _branchData.GetInChargesAsync(companyId);

            return _mapper.Map<IEnumerable<BranchInChargeListDTO>>(inCharges);
        }

        public async Task<BranchConsultDTO> PartialUpdateAsync(BranchPartialUpdateDTO dto)
        {
            ValidationHelper.EnsureValidId(dto.Id, "BranchId");

            var branch = await _data.GetByIdAsync(dto.Id);
            if (branch == null)
                throw new EntityNotFoundException(nameof(Branch), dto.Id);

            var allBranches = await _data.GetAllAsync();

            // --- Phone ---
            if (!string.IsNullOrWhiteSpace(dto.Phone) &&
                !StringHelper.EqualsNormalized(branch.Phone, dto.Phone))
            {
                bool phoneExists = allBranches.Any(c =>
                    c.Id != dto.Id &&
                    StringHelper.EqualsNormalized(c.Phone, dto.Phone));

                if (phoneExists)
                    throw new ValidationException("Phone", $"El telefono '{dto.Phone}' ya está en uso.");

                branch.Phone = dto.Phone;
            }

            await _data.UpdateAsync(branch);
            return _mapper.Map<BranchConsultDTO>(branch);
        }

        public async Task<BranchConsultDTO?> GetBranchByInChargeAsync(int userId)
        {
            ValidationHelper.EnsureValidId(userId, "User ID");

            var branch = await _branchData.GetBranchByInChargeAsync(userId);

            if (branch == null)
                return null;

            return _mapper.Map<BranchConsultDTO>(branch);
        }

        //Actions
=======
>>>>>>> parent of 845d2803 (solucion de errores)
        protected override Task BeforeCreateMap(BranchDTO dto, Branch entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override Task BeforeUpdateMap(BranchDTO dto, Branch entity)
        {
            ValidationHelper.ThrowIfEmpty(dto.Name, "Name");
            return Task.CompletedTask;
        }

        protected override async Task ValidateBeforeCreateAsync(BranchDTO dto)
        {
            var existing = await _data.GetAllAsync();
            if (existing.Any(e => StringHelper.EqualsNormalized(e.Name, dto.Name)))
                throw new ValidationException("Name", $"Ya existe un Branch con el Name '{dto.Name}'.");
        }

        protected override async Task ValidateBeforeUpdateAsync(BranchDTO dto, Branch existingEntity)
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
