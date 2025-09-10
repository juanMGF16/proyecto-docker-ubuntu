using Entity.Models.SecurityModule;
using Entity.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.System.Operating
{
    public class OperatingConsultDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public int OperationalGroupId { get; set; }
        public string OperationalGroupName { get; set; } = string.Empty;
    }
}
