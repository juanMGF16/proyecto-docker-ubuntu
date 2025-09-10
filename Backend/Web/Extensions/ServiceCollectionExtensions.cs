using Business.Services;
using Business.Services.Jwt;
using Business.Services.Jwt.Interfaces;
using Business.Services.JWTService;
using Data.DataINIT;
using Data.DataINIT.Generic;
using Data.DataINIT.Interface;
using Data.Repository.Interfaces.General;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Entity.Context;
using Entity.Models.SecurityModule;
using Microsoft.EntityFrameworkCore;

namespace Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AppDbContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

                return new AppDbContext(optionsBuilder.Options, configuration);
            });

            return services;
        }


        //Servicio de Sedders
        public static IServiceCollection AddDataSeeders(this IServiceCollection services)
        {
            // -----------------------
            // SecurityModule
            // -----------------------
            services.AddScoped<IDataSeeder>(provider =>
            new GenericSeeder<Person>("persons.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<User>("users.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Role>("roles.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Module>("modules.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Form>("forms.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Permission>("permissions.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<UserRole>("userRoles.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<FormModule>("formModules.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<RoleFormPermission>("roleFormPermissions.json", provider.GetRequiredService<IConfiguration>()));

            // -----------------------
            // ParametersModule
            // -----------------------


            // -----------------------
            // System
            // -----------------------
            services.AddScoped<IQrCodeService, QrCodeService>();

            services.AddScoped<GeneralSeeder>();

            services.AddScoped<AuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            return services;
        }
    }
}