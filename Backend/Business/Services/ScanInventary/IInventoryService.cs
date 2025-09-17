using Entity.DTOs.ScanItem;
using Entity.DTOs.System.Inventary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.ScanInventary
{
    public interface IInventoryService
    {
        /// <summary>
        /// Escanea un ítem mediante su código QR, valida y lo registra en un inventario abierto.
        /// </summary>
        Task<ScanItemResponseDTO> ScanItemAsync(ScanItemRequestDTO request);

        /// <summary>
        /// Obtiene el inventario activo de una zona.
        /// </summary>
        //Task<InventaryDTO?> GetActiveInventoryByZoneAsync(int zoneId);

        /// <summary>
        /// Cierra un inventario en curso y lo marca como "En verificación".
        /// </summary>
        //Task<bool> CloseInventoryAsync(int inventoryId, int userId, string observations);

        /// <summary>
        /// Lista todos los inventarios realizados por un grupo operativo.
        /// </summary>
        //Task<IEnumerable<InventaryDTO>> GetInventoriesByGroupAsync(int operatingGroupId);
    }
}
