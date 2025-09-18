using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Zone;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IZoneBusiness : IGenericBusiness<ZoneConsultDTO, ZoneDTO>
    {
        // General
        Task<IEnumerable<ZoneConsultDTO>> GetAllTotalAsync();

<<<<<<< HEAD
        //Specific
        Task<IEnumerable<ZoneSimpleDTO>> GetZonesByBranchAsync(int branchId);
        Task<ZoneDetailsDTO?> GetZoneDetailsAsync(int zoneId);
        Task<IEnumerable<ZoneInChargeListDTO>> GetInChargesAsync(int branchId);
        Task<ZoneConsultDTO?> GetZoneByAreaManagerAsync(int userId);
        Task<ZoneConsultDTO> PartialUpdateAsync(ZonePartialUpdateDTO dto);
=======
>>>>>>> parent of 845d2803 (solucion de errores)
    }
}
