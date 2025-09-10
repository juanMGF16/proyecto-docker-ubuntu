using Data.Repository.Interfaces.Specific.SecurityModule;
using Entity.Context;
using Entity.Models.SecurityModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.SecurityModule
{
    public class PersonData : GenericData<Person>, IPersonData
    {

        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public PersonData(AppDbContext context, ILogger<Person> logger) : base(context, logger) {
            _context = context;
            _logger = logger;
        }

        // Specific
        public async Task<IEnumerable<Person?>> GetAvailablePersons()
        {
            try
            {
                return await _context.Person
                    .Where(p => p.Active && !p.Users.Any())
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener personas disponibles");
                throw;
            }
        }

    }
}