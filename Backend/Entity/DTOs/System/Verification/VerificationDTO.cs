using Entity.Models.SecurityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.System.Verification
{
    public class VerificationDTO
    {
        public int Id { get; set; }
        public bool Result { get; set; } = true;
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        public int InventaryId { get; set; }
        public int UserId { get; set; }
    }
}
