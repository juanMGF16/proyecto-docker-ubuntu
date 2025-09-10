using Entity.DTOs.System.Zone;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IZoneBusiness : IGenericBusiness<ZoneConsultDTO, ZoneDTO>
    {
        // General
        Task<IEnumerable<ZoneConsultDTO>> GetAllTotalAsync();
        Task<IEnumerable<ZoneOperatingDTO>> GetAvailableZonesByUserAsync(int userId);
    }
}
