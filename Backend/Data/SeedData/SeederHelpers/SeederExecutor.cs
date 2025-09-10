using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.DataINIT.SeederHelpers
{
    public static class SeederExecutor
    {
        public static async Task SeedAllAsync(IServiceProvider services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString, b =>
                b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            var context = new AppDbContext(optionsBuilder.Options, config);

            // Aplicar migraciones antes de hacer el seed
            context.Database.Migrate();

            var seeder = services.GetRequiredService<GeneralSeeder>();
            await seeder.SeedAsync(context);
        }
    }
}