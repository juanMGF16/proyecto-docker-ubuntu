using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.SecurityModule;
using Entity.Models.System;

namespace Data.Repository.Interfaces.System
{
    public interface IItem : IGenericData<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsSpecific(int zonaId);
    }
}
