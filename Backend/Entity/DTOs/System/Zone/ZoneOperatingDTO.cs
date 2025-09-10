using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.System.Zone
{
    public class ZoneOperatingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
    }
}
