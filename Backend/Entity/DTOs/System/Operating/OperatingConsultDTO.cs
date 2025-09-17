namespace Entity.DTOs.System.Operating
{
    public class OperatingConsultDTO
    {
        public int Id { get; set; }
        public int OperatingId { get; set; }
        public string OperatingName { get; set; } = string.Empty;

        public int OperatingGroupId { get; set; }
        public string OperatingGroupName { get; set; } = string.Empty;
    }
}
