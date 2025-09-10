using Entity.Models.Base;
using Entity.Models.SecurityModule;

namespace Entity.Models.System
{
    public class Verification : BaseEntity
    {
        public bool Result { get; set; } = true;
        public DateTime Date { get; set; }
        public string Observations { get; set; } = string.Empty;

        // Claves Foraneas
        public int InventaryId { get; set; }
        public Inventary Inventary { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}