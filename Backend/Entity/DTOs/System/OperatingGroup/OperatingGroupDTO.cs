namespace Entity.DTOs.System.OperatingGroup
{
    public class OperatingGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        // Claves Foraneas
        public int AreaManagerId { get; set; }
    }
}
