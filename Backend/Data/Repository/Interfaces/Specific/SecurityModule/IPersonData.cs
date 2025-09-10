using Entity.Models.SecurityModule;

namespace Data.Repository.Interfaces.Specific.SecurityModule
{
    public interface IPersonData : IGenericData<Person> {
        Task<IEnumerable<Person?>> GetAvailablePersons();
    }
}