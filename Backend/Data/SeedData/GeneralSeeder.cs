using Data.DataINIT.Interface;
using Entity.Context;

namespace Data.DataINIT
{
    public class GeneralSeeder : IDataSeeder
    {
        private readonly IEnumerable<IDataSeeder> _seeders;

        public GeneralSeeder(IEnumerable<IDataSeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task SeedAsync(AppDbContext context)
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync(context);
            }
        }
    }

}
