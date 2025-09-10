using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Operating;
using Entity.DTOs.System.OperatingGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IOperatingGroupBusiness : IGenericBusiness<OperatingGroupConsultDTO, OperatingGroupDTO>
    {
        // General
        Task<IEnumerable<OperatingGroupConsultDTO>> GetAllTotalAsync();
    }
}
