using Entity.DTOs.System.Item;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.CargaMasiva
{
    public interface IItemBulkService
    {
        Task<ItemBulkUploadRequest> UploadExcelAsync(IFormFile file, int zoneId);
    }
}
