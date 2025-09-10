using Data.Repository.Implementations.System;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.Strategy;

namespace Data.Repository.Implementations.Strategy
{
    public class CascadeDeleteStrategy<T> : IDeleteStrategy<T> where T : class
    {
        public async Task<bool> DeleteAsync(int id, IGenericData<T> data)
        {
            if (data is CompanyData companyData)
            {
                return await companyData.KillCompanyAsync(id);
            }

            throw new NotSupportedException($"CascadeDeleteStrategy no soporta la entidad {typeof(T).Name}");
        }
    }
}
