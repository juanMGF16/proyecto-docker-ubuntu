namespace Business.Services.NITValidation.Interfaces
{
    public interface INitValidationService
    {
        Task<bool> ExistsAsync(string nitBase);
    }
}
