using Entity.DTOs.System.Checker;

namespace Business.Repository.Interfaces.Specific.System
{
    /// <summary>
    /// Define la lógica de negocio para la gestión de los usuarios asignados como Verificadores/Auditores.
    /// </summary>
    public interface ICheckerBusiness : IGenericBusiness<CheckerConsultDTO, CheckerDTO>
    {
        /// <summary>
        /// Obtiene la información del verificador asociado a un usuario por su ID de usuario.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        Task<CheckerConsultDTO> GetUserByIdAsync(int id);
    }
}