using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Entity.DTOs.CargaMasiva
{
    public class UploadExcelDTO
    {
        [Required]
        public IFormFile File { get; set; } = null!;

        [Required]
        public int ZoneId { get; set; }
    }
}
