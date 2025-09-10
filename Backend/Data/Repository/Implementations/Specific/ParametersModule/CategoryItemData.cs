using Data.Repository.Interfaces.Parameters;
using Entity.Context;
using Entity.Models.ParametersModule;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Parameters
{
    public class CategoryItemData : GenericData<CategoryItem>, ICategoryData
    {
        public CategoryItemData(AppDbContext context, ILogger<CategoryItem> logger) : base(context, logger) { }
    }
}
