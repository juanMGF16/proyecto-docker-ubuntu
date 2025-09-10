using Business.Repository.Implementations.Specific.Parameters;
using Business.Repository.Implementations.Specific.ParametersModule;
using Business.Repository.Implementations.Specific.SecurityModule;
using Business.Repository.Implementations.Specific.System;
using Business.Repository.Interfaces.Specific.Parameters;
using Business.Repository.Interfaces.Specific.ParametersModule;
using Business.Repository.Interfaces.Specific.SecurityModule;
using Business.Repository.Interfaces.Specific.System;
using Business.Services.CargaMasiva;
using Data.Repository.Implementations.Parameters;
using Data.Repository.Implementations.Specific;
using Data.Repository.Implementations.Specific.ParametersModule;
using Data.Repository.Implementations.Specific.SecurityModule;
using Data.Repository.Implementations.Specific.System;
using Data.Repository.Implementations.System;
using Data.Repository.Interfaces;
using Data.Repository.Interfaces.General;
using Entity.Models.ParametersModule;
using Entity.Models.SecurityModule;
using Entity.Models.System;

namespace Web.Extensions
{
    public static class  EntitiesServiceExtensions
    {
        public static IServiceCollection AddEntitiesServices(this IServiceCollection services)
        {
            // -----------------------
            // SecurityModule
            // -----------------------
            services.AddScoped<IPersonBusiness, PersonBusiness>();
            services.AddScoped<IGeneral<Person>, PersonData>();

            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IGeneral<User>, UserData>();

            services.AddScoped<IRoleBusiness, RoleBusiness>();
            services.AddScoped<IGeneral<Role>, RoleData>();

            services.AddScoped<IFormBusiness, FormBusiness>();
            services.AddScoped<IGeneral<Form>, FormData>();

            services.AddScoped<IModuleBusiness, ModuleBusiness>();
            services.AddScoped<IGeneral<Module>, ModuleData>();

            services.AddScoped<IPermissionBusiness, PermissionBusiness>();
            services.AddScoped<IGeneral<Permission>, PermissionData>();

            services.AddScoped<IUserRoleBusiness, UserRoleBusiness>();
            services.AddScoped<IGeneral<UserRole>, UserRoleData>();

            services.AddScoped<IFormModuleBusiness, FormModuleBusiness>();
            services.AddScoped<IGeneral<FormModule>, FormModuleData>();

            services.AddScoped<IRoleFormPermissionBusiness, RoleFormPermissionBusiness>();
            services.AddScoped<IGeneral<RoleFormPermission>, RoleFormPermissionData>();

            // -----------------------
            // ParametersModule
            // -----------------------
            services.AddScoped<ICategoryBusiness, CategoryItemBusiness>();
            services.AddScoped<IGeneral<CategoryItem>, CategoryItemData>();

            services.AddScoped<IStateItemBusiness, StateItemBusiness>();
            services.AddScoped<IGeneral<StateItem>, StateItemData>();

            services.AddScoped<INotificationBusiness, NotificationBusiness>();
            services.AddScoped<IGeneral<Notification>, NotificationData>();

            // -----------------------
            // System
            // -----------------------
            services.AddScoped<IItemBusiness, ItemBusiness>();
            services.AddScoped<IGeneral<Item>, ItemData>();
            services.AddScoped<IGenericData<Item>, ItemData>();

            services.AddScoped<ICompanyBusiness, CompanyBusiness>();
            services.AddScoped<IGeneral<Company>, CompanyData>();

            services.AddScoped<IBranchBusiness, BranchBusiness>();
            services.AddScoped<IGeneral<Branch>, BranchData>();

            services.AddScoped<IZoneBusiness, ZoneBusiness>();
            services.AddScoped<IGeneral<Zone>, ZoneData>();

            services.AddScoped<IInventaryBusiness, InventaryBusiness>();
            services.AddScoped<IGeneral<Inventary>, InventaryData>();

            services.AddScoped<IInventaryDetailBusiness, InventaryDetailBusiness>();
            services.AddScoped<IGeneral<InventaryDetail>, InventaryDetailData>();

            services.AddScoped<IOperatingBusiness, OperatingBusiness>();
            services.AddScoped<IGeneral<Operating>, OperatingData>();

            services.AddScoped<IOperatingGroupBusiness, OperatingGroupBusiness>();
            services.AddScoped<IGeneral<OperatingGroup>, OperatingGroupData>();

            services.AddScoped<IVerificationBusiness, VerificationBusiness>();
            services.AddScoped<IGeneral<Verification>, VerificationData>();

            // -----------------------
            // Carga Masiva
            // -----------------------

            services.AddScoped<IItemBulkService, ItemBulkService>();


            return services;
        }
    }
}
