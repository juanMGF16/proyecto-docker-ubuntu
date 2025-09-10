using Business.Repository.Implementations.Specific.System.Others;
using Business.Repository.Interfaces.Specific.System.Others;
using Business.Services;
using Business.Services.Jwt.Interfaces;
using Business.Services.Jwt;
using Business.Services.JWTService;
using Business.Services.JWTService.Interfaces;
using Business.Services.NITValidation;
using Business.Services.NITValidation.Interfaces;
using Business.Services.PaswordRecovery;
using Business.Services.PaswordRecovery.Interfaces;
using Business.Services.SendEmail;
using Business.Services.SendEmail.Interfaces;
using Data.Factory;
using Data.Repository.Implementations.Specific.System.Others;
using Data.Repository.Implementations.Strategy;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Specific.System.Others;
using Data.Repository.Interfaces.Strategy;
using Entity.Models.ParametersModule;

namespace Web.Extensions
{
    public static class BusinessServiceExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // =============== [ Email Service ] ===============
            var emailSettings = configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(emailSettings);
            services.AddScoped<IEmailService, EmailService>();

            // =============== [ JWT Service ] ===============
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<AuthService>();

            // =============== [ Factory ] ===============
            services.AddScoped<IDataFactoryGlobal, GlobalFactory>();

            // =============== [ Strategy Services ] ===============
            services.AddScoped(typeof(LogicalDeleteStrategy<>));
            services.AddScoped(typeof(PermanentDeleteStrategy<>));
            services.AddScoped(typeof(CascadeDeleteStrategy<>));
            services.AddScoped(typeof(IDeleteStrategyResolver<>), typeof(DeleteStrategyResolver<>));

            // =============== [ Extra Utils ] ===============
            services.AddScoped<IQrCodeService, QrCodeService>();
            services.AddScoped<IPasswordRecoveryService, PasswordRecoveryService>();
            services.AddHttpClient<INitValidationService, NitValidationService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // =============== [ Others ] ===============
            services.AddScoped<IDashboardData, DashboardData>();
            services.AddScoped<IDashboardBusiness, DashboardBusiness>();


            return services;
        }
    }
}
