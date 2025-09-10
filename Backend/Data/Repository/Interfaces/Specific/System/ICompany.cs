using Entity.Models.System;

namespace Data.Repository.Interfaces.System
{
    public interface ICompany : IGenericData<Company> {
        //Specific
        Task<bool> KillCompanyAsync(int companyId);
    }
}
