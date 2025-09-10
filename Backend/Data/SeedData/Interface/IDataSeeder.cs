using Entity.Context;

namespace Data.DataINIT.Interface
{
    public interface IDataSeeder
    {
        Task SeedAsync(AppDbContext context);
    }
}