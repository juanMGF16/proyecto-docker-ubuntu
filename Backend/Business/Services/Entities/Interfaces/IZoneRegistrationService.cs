using Entity.DTOs.System.Zone.NestedCreation;

namespace Business.Services.Entities.Interfaces
{
    public interface IZoneRegistrationService
    {
        Task<ZoneCreateResponseDTO> CreateZoneWithEncZoneAsync(ZoneCreateRequestDTO request);
    }
}
