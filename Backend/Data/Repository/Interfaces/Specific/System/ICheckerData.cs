using Entity.Models.System;

namespace Data.Repository.Interfaces.Specific.System
{
    /// <summary>
    /// Repositorio para verificadores
    /// </summary>
    public interface ICheckerData : IGenericData<Checker>
    {
        /// <summary>
        /// Obtiene un verificador por ID de usuario
        /// </summary>
        Task<Checker?> GetByUserIdAsync(int id);
    }
}
