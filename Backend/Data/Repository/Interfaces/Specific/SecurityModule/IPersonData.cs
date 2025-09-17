using Entity.Models.SecurityModule;

namespace Data.Repository.Interfaces.Specific.SecurityModule
{
    public interface IPersonData : IGenericData<Person> {
        Task<IEnumerable<Person?>> GetAvailablePersons();
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentExistsAsync(string documentType, string documentNumber);
        Task<bool> PhoneExistsAsync(string phone);
    }
}