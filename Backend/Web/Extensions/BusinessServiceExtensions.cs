<<<<<<< HEAD
﻿using Business.Repository.Implementations.Specific.System.Others;
using Business.Repository.Interfaces.Specific.System.Others;
using Business.Services;
using Business.Services.Jwt;
using Business.Services.Jwt.Interfaces;
using Business.Services.JWTService;
using Business.Services.JWTService.Interfaces;
using Business.Services.NITValidation;
using Business.Services.NITValidation.Interfaces;
using Business.Services.PaswordRecovery;
using Business.Services.PaswordRecovery.Interfaces;
using Business.Services.ScanInventary;
=======
﻿using Business.Services;
using Business.Services.JWTService;
using Business.Services.JWTService.Interfaces;
>>>>>>> parent of 845d2803 (solucion de errores)
using Business.Services.SendEmail;
using Business.Services.SendEmail.Interfaces;
using Data.Factory;
using Data.Repository.Implementations.Strategy;
using Data.Repository.Implementations.System;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.General;
using Data.Repository.Interfaces.Strategy;
using Entity.Models.ParametersModule;
using Business.Factory;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Business.Services.CredentialGenerator.Interfaces;
using Business.Services.CredentialGenerator;
using Business.Services.Entities.Implementations;
using Business.Services.Entities.Interfaces;

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
            services.AddScoped<IDataFactoryGlobal, GlobalFactory>(); // Data
            services.AddScoped<IBusinessServiceFactory, BusinessServiceFactory>(); // Business

            // =============== [ Factory Bussiness Service ] ===============
            services.AddScoped<IBranchRegistrationService, BranchRegistrationService>();
            services.AddScoped<IZoneRegistrationService, ZoneRegistrationService>();

            // =============== [ Strategy Services ] ===============
            services.AddScoped(typeof(LogicalDeleteStrategy<>));
            services.AddScoped(typeof(PermanentDeleteStrategy<>));
            services.AddScoped(typeof(IDeleteStrategyResolver<>), typeof(DeleteStrategyResolver<>));

            // =============== [ Extra Utils ] ===============
            services.AddScoped<IQrCodeService, QrCodeService>();
<<<<<<< HEAD
            services.AddScoped<IPasswordRecoveryService, PasswordRecoveryService>();
            services.AddHttpClient<INitValidationService, NitValidationService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ICredentialGeneratorService, CredentialGeneratorService>();
            services.AddScoped<IInventoryService, InventoryService>();

            // =============== [ Others ] ===============
            services.AddScoped<IDashboardData, DashboardData>();
            services.AddScoped<IDashboardBusiness, DashboardBusiness>();
=======
>>>>>>> parent of 845d2803 (solucion de errores)

            return services;
        }
    }
}
