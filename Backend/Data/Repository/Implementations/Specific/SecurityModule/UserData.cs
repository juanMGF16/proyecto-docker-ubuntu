using Data.Repository.Interfaces.Specific.SecurityModule;
using Entity.Context;
using Entity.Models.SecurityModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repository.Implementations.Specific.SecurityModule
{
    public class UserData : GenericData<User>, IUserData
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public UserData(AppDbContext context, ILogger<User> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.User
                    .Include(u => u.Person)
                    .Where(u => u.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "No se puedieron obtener los datos");
                throw;
            }
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.User
                    .Include(u => u.Person)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedieron obtener los datos por id: {id}");
                throw;
            }
        }

        // General
        public override async Task<IEnumerable<User>> GetAllTotalAsync()
        {
            try
            {
                return await _context.User
                    .Include(u => u.Person)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedo obtener todos los datos");
                throw;
            }

        }

        // Specific
        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                return await _context.User
                    .Include(u => u.Person)
                    .FirstOrDefaultAsync(u => u.Username == username && u.Active);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"No se puedo obtener el User con Username: {username}");
                throw;
            }
        }

        public async Task<bool> HasCompanyAsync(int userId)
        {
            try
            {
                return await _context.Company.AnyAsync(c => c.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verificando si el usuario con id {userId} tiene Company asociada.");
                throw;
            }
        }
    }
}