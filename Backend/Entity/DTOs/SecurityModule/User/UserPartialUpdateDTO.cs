namespace Entity.DTOs.SecurityModule.User
{
    public class UserPartialUpdateDTO
    {
        public int Id { get; set; } 
        public string? Username { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
