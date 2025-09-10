using Data.Repository.Interfaces.Specific;
using Entity.Context;
using Entity.Models.SecurityModule;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific
{
    public class FormData : GenericData<Form>, IFormData
    {
        public FormData(AppDbContext context, ILogger<Form> logger) : base(context, logger) {}
    }
}