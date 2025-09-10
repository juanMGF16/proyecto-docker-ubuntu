using Data.Repository.Interfaces.Specific.SecurityModule;
using Entity.Context;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.SecurityModule
{
    public class ModuleData : GenericData<Module>, IModuleData
    {
        public ModuleData(AppDbContext context, ILogger<Module> logger) : base(context, logger) {}
    }
}