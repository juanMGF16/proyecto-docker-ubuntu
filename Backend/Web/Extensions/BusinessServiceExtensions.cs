using Business.Services;
using Business.Services.JWTService;
using Business.Services.JWTService.Interfaces;
using Business.Services.SendEmail;
using Business.Services.SendEmail.Interfaces;
using Data.Factory;
using Data.Repository.Implementations.Strategy;
using Data.Repository.Implementations.System;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.General;
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
            services.AddScoped(typeof(IDeleteStrategyResolver<>), typeof(DeleteStrategyResolver<>));

            // =============== [ Extra Utils ] ===============
            services.AddScoped<IQrCodeService, QrCodeService>();

            return services;
        }
    }
}
