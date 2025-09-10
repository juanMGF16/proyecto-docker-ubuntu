using AutoMapper;
using Entity.DTOs.ParametersModels.Category;
using Entity.DTOs.ParametersModels.Notification;
using Entity.DTOs.SecurityModule;
using Entity.DTOs.SecurityModule.FormModule;
using Entity.DTOs.SecurityModule.Person;
using Entity.DTOs.SecurityModule.RoleFormPermission;
using Entity.DTOs.SecurityModule.User;
using Entity.DTOs.SecurityModule.UserRole;
using Entity.DTOs.System;
using Entity.DTOs.System.Branch;
using Entity.DTOs.System.Company;
using Entity.DTOs.System.Inventary;
using Entity.DTOs.System.InventaryDetail;
using Entity.DTOs.System.Item;
using Entity.DTOs.System.Operating;
using Entity.DTOs.System.OperatingGroup;
using Entity.DTOs.System.Verification;
using Entity.DTOs.System.Zone;
using Entity.Models.ParametersModule;
using Entity.Models.SecurityModule;
using Entity.Models.System;
using Utilities.Enums.Models;

namespace Business.AutoMapper
{
    public class GeneralMapper : Profile
    {
        public GeneralMapper()
        {

            // -----------------------
            // SecurityModule
            // -----------------------
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<Person, PersonAvailableDTO>().ReverseMap();

            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Form, FormDTO>().ReverseMap();
            CreateMap<Module, ModuleDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();


            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserOptionsDTO>().ReverseMap();


            CreateMap<UserRole, UserRoleDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();
            CreateMap<UserRole, UserRoleOptionsDTO>().ReverseMap();

            CreateMap<FormModule, FormModuleDTO>()
                .ReverseMap();
            CreateMap<FormModule, FormModuleOptionsDTO>().ReverseMap();

            CreateMap<RoleFormPermission, RoleFormPermissionDTO>()
                .ReverseMap();
            CreateMap<RoleFormPermission, RoleFormPermissionOptionsDTO>().ReverseMap();

            // -----------------------
            // ParametersModule
            // -----------------------
            CreateMap<CategoryItem, CategoryItemDTO>().ReverseMap();
            CreateMap<StateItem, StateItemDTO>().ReverseMap();

            //CreateMap<Notification, NotificationDTO>().ReverseMap();
            //CreateMap<Notification, NotificationOptionsDTO>().ReverseMap();

            // -----------------------
            // System
            // -----------------------
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Item, ItemConsultDTO>()
                .ForMember(dest => dest.StateItemName, opt => opt.MapFrom(src => src.StateItem.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryItem.Name))
                .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.Zone.Name))
                .ReverseMap();
            CreateMap<Item, ItemConsultCategoryDTO>();



            CreateMap<Branch, BranchDTO>().ReverseMap();
            CreateMap<Branch, BranchConsultDTO>()
                .ForMember(dest => dest.InChargeId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.InChargeName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();

            CreateMap<Company, CompanyDTO>()
            .ForMember(dest => dest.IndustryId, opt => opt.MapFrom(src => (int)src.Industry))
            .ReverseMap()
            .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => (TypeIndustry)src.IndustryId));

            CreateMap<Company, CompanyConsultDTO>()
                .ForMember(dest => dest.IndustryId, opt => opt.MapFrom(src => (int)src.Industry))
                .ForMember(dest => dest.IndustryName, opt => opt.MapFrom(src => src.Industry.ToString()))
                .ForMember(dest => dest.OwnerUserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OwnerUserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap()
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => (TypeIndustry)src.IndustryId));

            CreateMap<Zone, ZoneDTO>().ReverseMap();
            CreateMap<Zone, ZoneConsultDTO>()
                .ForMember(dest => dest.InChargeName, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.InChargeId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
                .ReverseMap();

            CreateMap<Inventary, InventaryDTO>().ReverseMap();
            CreateMap<Inventary, InventaryConsultDTO>()
                .ForMember(dest => dest.OperatingGroupName, opt => opt.MapFrom(src => src.OperatingGroup.Name))
                .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.Zone.Name))
                .ReverseMap();

            CreateMap<InventaryDetail, InventaryDetailDTO>().ReverseMap();
            CreateMap<InventaryDetail, InventaryDetailConsultDTO>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.StateItemName, opt => opt.MapFrom(src => src.StateItem.Name))
                .ForMember(dest => dest.InventaryObservations, opt => opt.MapFrom(src => src.Inventary.Observations))
                .ReverseMap();

            CreateMap<Operating, OperatingDTO>().ReverseMap();
            CreateMap<Operating, OperatingConsultDTO>()
                .ForMember(dest => dest.OperationalGroupName, opt => opt.MapFrom(src => src.OperationalGroup.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();

            CreateMap<OperatingGroup, OperatingGroupDTO>().ReverseMap();
            CreateMap<OperatingGroup, OperatingGroupConsultDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();

            CreateMap<Verification, VerificationDTO>().ReverseMap();
            CreateMap<Verification, VerificationConsultDTO>()
                .ForMember(dest => dest.InventaryObservations, opt => opt.MapFrom(src => src.Inventary.Observations))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();
        }
    }
}