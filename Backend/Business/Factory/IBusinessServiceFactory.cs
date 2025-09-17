using Business.Services.Entities.Interfaces;

namespace Business.Factory
{
    public interface IBusinessServiceFactory
    {
        IBranchRegistrationService CreateBranchRegistrationService();
        IZoneRegistrationService CreateZoneRegistrationService();
    }
}
