using Data.DataINIT;
using Data.DataINIT.Generic;
using Data.DataINIT.Interface;
using Entity.Context;
using Entity.Models.ParametersModule;
using Entity.Models.SecurityModule;
using Entity.Models.System;
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
                new GenericSeeder<Person>("SecurityModule", "persons.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<User>("SecurityModule", "users.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Role>("SecurityModule", "roles.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Module>("SecurityModule", "modules.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Form>("SecurityModule", "forms.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Permission>("SecurityModule", "permissions.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<UserRole>("SecurityModule", "userRoles.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<FormModule>("SecurityModule", "formModules.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<RoleFormPermission>("SecurityModule", "roleFormPermissions.json", provider.GetRequiredService<IConfiguration>()));

            // -----------------------
            // ParametersModule
            // -----------------------
            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<CategoryItem>("ParametersModule", "categoryItem.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<StateItem>("ParametersModule", "stateItem.json", provider.GetRequiredService<IConfiguration>()));


            // -----------------------
            // System
            // -----------------------
            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Company>("System", "company.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Branch>("System", "branch.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Zone>("System", "zone.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Item>("System", "item.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<OperatingGroup>("System", "operatingGroup.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Operating>("System", "operating.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Inventary>("System", "inventary.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<InventaryDetail>("System", "inventaryDetail.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<IDataSeeder>(provider =>
                new GenericSeeder<Verification>("System", "verification.json", provider.GetRequiredService<IConfiguration>()));

            services.AddScoped<GeneralSeeder>();
            return services;
        }

    }
}