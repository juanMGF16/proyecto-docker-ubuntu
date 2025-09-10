using Entity.DTOs.SecurityModule.Person;

namespace Business.Repository.Interfaces.Specific.SecurityModule
{
    public interface IPersonBusiness : IGenericBusiness<PersonDTO, PersonDTO>
    {
        // General
        Task<IEnumerable<PersonDTO>> GetAllTotalPersonsAsync();

        // Specific
        Task<IEnumerable<PersonAvailableDTO?>> GetPersonAvailableAsync();
    }
}