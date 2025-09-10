using Entity.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.System.Inventary
{
    public class InventaryConsultDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; } = string.Empty;

        public int ZoneId { get; set; }
        public string ZoneName { get; set; } = string.Empty;

        public int OperatingGroupId { get; set; }
        public string OperatingGroupName { get; set; } = string.Empty;

    }
}
