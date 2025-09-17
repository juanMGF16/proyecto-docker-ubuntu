using Entity.Models.System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repository.Interfaces.System
{
    public interface IBranch : IGenericData<Branch> {
        //Contexto para transcciones
        Task<IDbContextTransaction> BeginTransactionAsync();

        //Specific
        Task<IEnumerable<Branch>> GetBranchesByCompanyAsync(int companyId);
        Task<Branch?> GetBranchWithZonesAndItemsAsync(int branchId);
        Task<Branch?> GetInChargeAsync(int branchId);
        Task<IEnumerable<Branch>> GetInChargesAsync(int companyId);
        Task<Branch?> GetBranchByInChargeAsync(int userId);
    }
}
