using Business.Services.ScanInventary;
using Entity.Context;
using Entity.DTOs.ScanItem;
using Entity.Models.System;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ScanItemResponseDTO> ScanItemAsync(ScanItemRequestDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Buscar ítem (incluyendo navegación)
                var item = await _context.Item
                    .Include(i => i.Zone)
                    .Include(i => i.CategoryItem)
                    .Include(i => i.StateItem)
                    .FirstOrDefaultAsync(i => i.Code == request.Code);

                if (item == null)
                    return new ScanItemResponseDTO { Success = false, Message = "El ítem no existe." };

                // 2. Validar inventario activo
                var inventory = await _context.Inventary
                    .Include(inv => inv.InventaryDetails)
                    .FirstOrDefaultAsync(inv => inv.ZoneId == request.ZoneId && inv.Active);

                if (inventory == null)
                    return new ScanItemResponseDTO { Success = false, Message = "No hay inventario abierto en esta zona." };

                // 3. Prevenir duplicados
                if (inventory.InventaryDetails.Any(d => d.ItemId == item.Id))
                    return new ScanItemResponseDTO { Success = false, Message = "Este ítem ya fue registrado." };

                // 4. Validar zona
                string? alert = null;
                if (item.ZoneId != request.ZoneId)
                    alert = $"El ítem pertenece a la zona {item.Zone.Name}, no a la actual.";

                // 5. Registrar detalle
                var detail = new InventaryDetail
                {
                    InventaryId = inventory.Id, // 👈 corregido (antes pusiste Id = inventory.Id)
                    ItemId = item.Id,
                    StateItemId = request.StateItemId ?? item.StateItemId,
                    Active = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = request.UserId.ToString()
                };

                _context.InventaryDetail.Add(detail);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // 6. Respuesta enriquecida
                return new ScanItemResponseDTO
                {
                    Success = true,
                    Message = "Ítem registrado correctamente.",
                    InventoryDetailId = detail.Id,
                    ItemName = item.Name,
                    Category = item.CategoryItem.Name,
                    Alert = alert
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
