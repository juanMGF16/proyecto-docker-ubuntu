using Entity.DTOs.System.Verification;
using Entity.DTOs.System.Verification.AreaManager;


namespace Business.Repository.Interfaces.Specific.System
{
    /// <summary>
    /// Define la lógica de negocio para la gestión de las verificaciones o auditorías post-inventario.
    /// </summary>
    public interface IVerificationBusiness : IGenericBusiness<VerificationConsultDTO, VerificationDTO>
    {
        // General

        /// <summary>
        /// Obtiene todos los registros de verificación, incluyendo los inactivos.
        /// </summary>
        Task<IEnumerable<VerificationConsultDTO>> GetAllTotalAsync();

        /// <summary>
        /// Obtiene los detalles completos de la verificación asociada a un inventario.
        /// </summary>
        /// <param name="inventoryId">ID del registro de inventario a verificar.</param>
        Task<VerificationDetailResponseDTO> GetVerificationDetailAsync(int inventoryId);
    }
}