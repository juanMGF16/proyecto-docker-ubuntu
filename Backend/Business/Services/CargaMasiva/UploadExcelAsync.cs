using ClosedXML.Excel;
using Data.Repository.Interfaces;
using Entity.Context;
using Entity.DTOs.System.Item;
using Entity.Models.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.CargaMasiva
{
    public class ItemBulkService : IItemBulkService
    {
        private readonly IGenericData<Item> _itemRepository;
        private readonly AppDbContext _context; // solo para consultar Category y State

        public ItemBulkService(IGenericData<Item> itemRepository, AppDbContext context)
        {
            _itemRepository = itemRepository;
            _context = context;
        }

        public async Task<ItemBulkUploadRequest> UploadExcelAsync(IFormFile file, int zoneId)
        {
            var result = new ItemBulkUploadRequest();

            if (file == null || file.Length == 0)
            {
                result.Errors.Add("Archivo vacío o no válido.");
                return result;
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1) ?? throw new Exception("La hoja 1 no existe en el archivo Excel.");
            var range = worksheet.RangeUsed() ?? throw new Exception("La hoja de Excel está vacía.");
            var rows = range.RowsUsed().Skip(1);

            result.TotalRows = rows.Count();

            foreach (var row in rows)
            {
                try
                {
                    string code = row.Cell(1).GetString();
                    string name = row.Cell(2).GetString();
                    string description = row.Cell(3).GetString();
                    string categoryName = row.Cell(4).GetString();
                    string stateName = row.Cell(5).GetString();

                    var category = await _context.Category.FirstOrDefaultAsync(c => c.Name == categoryName);
                    var state = await _context.StateItem.FirstOrDefaultAsync(s => s.Name == stateName);

                    if (category == null || state == null)
                    {
                        result.Errors.Add($"Fila {row.RowNumber()}: categoría '{categoryName}' o estado '{stateName}' no encontrado.");
                        result.Failed++;
                        continue;
                    }

                    var item = new Item
                    {
                        Code = code,
                        Name = name,
                        Description = description,
                        CategoryItemId = category.Id,
                        StateItemId = state.Id,
                        ZoneId = zoneId
                    };

                    // 🔹 Aquí usamos CreateAsync para respetar la lógica de ItemData (QR)
                    await _itemRepository.CreateAsync(item);

                    result.Inserted++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Fila {row.RowNumber()}: {ex.Message}");
                    result.Failed++;
                }
            }

            return result;
        }
    }
}
