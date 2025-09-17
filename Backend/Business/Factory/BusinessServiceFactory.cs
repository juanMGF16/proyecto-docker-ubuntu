using Business.Services.Entities.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Factory
{
    public class BusinessServiceFactory : IBusinessServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BusinessServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IBranchRegistrationService CreateBranchRegistrationService()
        {
            // El contenedor resuelve todo automáticamente
            return _serviceProvider.GetRequiredService<IBranchRegistrationService>();
        }

        public IZoneRegistrationService CreateZoneRegistrationService()
        {
            // El contenedor resuelve todo automáticamente
            return _serviceProvider.GetRequiredService<IZoneRegistrationService>();
        }
    }
}
