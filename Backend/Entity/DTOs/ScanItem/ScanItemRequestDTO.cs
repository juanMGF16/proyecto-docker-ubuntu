using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ScanItem
{
    public class ScanItemRequestDTO
    {
        public string Code { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int ZoneId { get; set; }
        public int? StateItemId { get; set; }
    }
}
