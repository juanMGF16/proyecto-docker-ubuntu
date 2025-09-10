using Entity.Models.SecurityModule;
using Entity.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.System.Operating
{
    public class OperatingDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationalGroupId { get; set; }
    }
}
