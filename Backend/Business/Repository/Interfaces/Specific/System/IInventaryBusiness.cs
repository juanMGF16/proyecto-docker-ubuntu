using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Inventary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IInventaryBusiness : IGenericBusiness<InventaryConsultDTO, InventaryDTO>
    {
        // General
        Task<IEnumerable<InventaryConsultDTO>> GetAllTotalAsync();
    }
}
