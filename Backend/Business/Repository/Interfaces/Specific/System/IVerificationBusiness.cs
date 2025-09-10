using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.Interfaces.Specific.System
{
    public interface IVerificationBusiness : IGenericBusiness<VerificationConsultDTO, VerificationDTO>
    {
        // General
        Task<IEnumerable<VerificationConsultDTO>> GetAllTotalAsync();
    }
}
