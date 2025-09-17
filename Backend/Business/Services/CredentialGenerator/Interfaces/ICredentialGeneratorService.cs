namespace Business.Services.CredentialGenerator.Interfaces
{
    public interface ICredentialGeneratorService
    {
        (string username, string password) GenerateCredentials(string name, string lastName, string email);
    }
}
