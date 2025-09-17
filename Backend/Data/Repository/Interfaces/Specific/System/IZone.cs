using Entity.Models.System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repository.Interfaces.System
{
    public interface IZone : IGenericData<Zone> {
        //Contexto para transcciones
        Task<IDbContextTransaction> BeginTransactionAsync();

        //Specific
        Task<IEnumerable<Zone>> GetZonesByBranchAsync(int branchId);
        Task<Zone?> GetZoneDetailsAsync(int zoneId);
        Task<IEnumerable<Zone>> GetInChargesAsync(int branchId);
        Task<Zone?> GetZoneByAreaManagerAsync(int userId);
    }
}
