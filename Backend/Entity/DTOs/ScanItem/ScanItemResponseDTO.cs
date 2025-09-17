using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ScanItem
{
    public class ScanItemResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? InventoryDetailId { get; set; }
        public string? Alert { get; set; }
        public string? ItemName { get; set; }  
        public string? Category { get; set; }
    }
}
