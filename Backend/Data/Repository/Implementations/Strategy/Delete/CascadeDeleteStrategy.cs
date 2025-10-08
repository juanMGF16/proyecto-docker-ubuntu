using Data.Repository.Implementations.Specific.System;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Strategy.Delete;
using Utilities.Common;

namespace Data.Repository.Implementations.Strategy.Delete
{
    /// <summary>
    /// Estrategia de eliminación en cascada para entidades con dependencias complejas
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public class CascadeDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    {
        private readonly IUserContextService _userContext;

        public CascadeDeleteStrategy(IUserContextService userContext)
        {
            _userContext = userContext;
        }

        /// <summary>
        /// Ejecuta eliminación en cascada eliminando todas las dependencias de la entidad
        /// </summary>
        /// <param name="id">ID de la entidad a eliminar</param>
        /// <param name="data">Repositorio de datos</param>    /// <summary>
        /// Ejecuta eliminación en cascada eliminando todas las dependencias de la entidad
        /// </summary>
        /// <param name="id">ID de la entidad a eliminar</param>
        /// <param name="data">Repositorio de datos</param>
        public async Task<bool> DeleteAsync(int id, IGenericData<T> data)
        {
            if (data is CompanyData companyData)
            {
                int currentUserId = _userContext.GetCurrentUserId();
                return await companyData.KillCompanyAsync(id, currentUserId);
            }

            throw new NotSupportedException($"CascadeDeleteStrategy no soporta la entidad {typeof(T).Name}");
        }
    }

}
