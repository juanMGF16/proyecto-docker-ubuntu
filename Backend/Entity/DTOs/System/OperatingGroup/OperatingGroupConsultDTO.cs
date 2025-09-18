namespace Entity.DTOs.System.OperatingGroup
{
    public class OperatingGroupConsultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        // Claves Foraneas
<<<<<<< HEAD
        public int AreaManagerId { get; set; }
        public string AreaManagerName { get; set; } = string.Empty;
=======
        public int UserId { get; set; }
        public string UserName { get; set; } 
>>>>>>> parent of 845d2803 (solucion de errores)

    }
}
