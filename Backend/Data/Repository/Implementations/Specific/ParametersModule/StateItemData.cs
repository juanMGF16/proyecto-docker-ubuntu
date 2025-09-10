using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository.Interfaces.Specific;
using Data.Repository.Interfaces.System;
using Entity.Context;
using Entity.Models.ParametersModule;
using Entity.Models.SecurityModule;
using Entity.Models.System;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.System
{
    public class StateItemData : GenericData<StateItem>, IStateItemData
    {
        public StateItemData(AppDbContext context, ILogger<StateItem> logger) : base(context, logger) { }
    }
}
