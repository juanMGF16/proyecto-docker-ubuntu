using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.ParametersModule;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.System
{
    public class StateItemData : GenericData<StateItem>, IStateItemData
    {
        public StateItemData(AppDbContext context, ILogger<StateItem> logger) : base(context, logger) { }
    }
}
