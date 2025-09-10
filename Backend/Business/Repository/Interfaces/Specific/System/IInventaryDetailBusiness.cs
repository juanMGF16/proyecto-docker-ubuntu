using Entity.DTOs.System.Branch;
using Entity.DTOs.System.InventaryDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IInventaryDetailBusiness : IGenericBusiness<InventaryDetailConsultDTO, InventaryDetailDTO>
    {
        // General
        Task<IEnumerable<InventaryDetailConsultDTO>> GetAllTotalAsync();
    }
}
