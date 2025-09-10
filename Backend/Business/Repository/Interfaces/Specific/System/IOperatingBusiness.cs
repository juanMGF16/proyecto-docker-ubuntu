using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Operating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IOperatingBusiness : IGenericBusiness<OperatingConsultDTO, OperatingDTO>
    {
        // General
        Task<IEnumerable<OperatingConsultDTO>> GetAllTotalAsync();
    }
}
