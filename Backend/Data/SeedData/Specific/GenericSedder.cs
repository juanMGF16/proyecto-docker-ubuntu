using System.Text.Json;
using Data.DataINIT.Interface;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utilities.Helpers.Interface;

namespace Data.DataINIT.Generic
{
    public class GenericSeeder<T> : IDataSeeder where T : class
    {
        private readonly string _fileName;
        private readonly IConfiguration _configuration;

        public GenericSeeder(string fileName, IConfiguration configuration)
        {
            _fileName = fileName;
            _configuration = configuration;
        }

        public async Task SeedAsync(AppDbContext context)
        {
            var basePath = _configuration["SeedDataPath"] ??
                           Path.Combine(AppContext.BaseDirectory, "DataINIT", "Specific", "SeedData");

            var filePath = Path.Combine(basePath, _fileName);

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Seeder] Archivo no encontrado: {filePath}");
                return;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data is { Count: > 0 })
            {
                // Post-procesamiento si implementa IRequiresPasswordHashing
                foreach (var item in data)
                {
                    if (item is IRequiresPasswordHashing hashable)
                    {
                        hashable.HashPassword();
                    }
                }

                var dbSet = context.Set<T>();
                if (!await dbSet.AnyAsync())
                {
                    await dbSet.AddRangeAsync(data);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[Seeder] Datos insertados para: {typeof(T).Name}");
                }
            }
        }
    }

}
