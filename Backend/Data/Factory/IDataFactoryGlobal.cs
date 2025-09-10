using Data.Repository.Interfaces.Parameters;
using Data.Repository.Interfaces.Specific;
using Data.Repository.Interfaces.Specific.ParametersModule;
using Data.Repository.Interfaces.Specific.SecurityModule;
using Data.Repository.Interfaces.Specific.System;
using Data.Repository.Interfaces.System;

namespace Data.Factory
{
    public interface IDataFactoryGlobal
    {
        // -----------------------
        // SecurityModule
        // -----------------------
        IPersonData CreatePersonData();
        IUserData CreateUserData();
        IRoleData CreateRoleData();
        IFormData CreateFormData();
        IModuleData CreateModuleData();
        IPermissionData CreatePermissionData();
        IUserRoleData CreateUserRoleData();
        IFormModuleData CreateFormModuleData();
        IRoleFormPermissionData CreateRoleFormPermissionData();

        // -----------------------
        // ParametersModule
        // -----------------------
        ICategoryData CreateCategoryData();
        IStateItemData CreateStateItemData();
        INotificationData CreateNotificationData();

        // -----------------------
        // System
        // -----------------------
        IItem CreateItem();
        ICompany CreateCompanyData();
        IBranch CreateBranchData();
        IZone CreateZoneData();
        IInventary CreateInventaryData();
        IInventaryDetail CreateInventaryDetailData();
        IOperating CreateOperatingData();
        IOperatingGroup CreateOperatingGroupData();
        IVerification CreateVerificationData();

        // Ejemplo de Generico
        // IGenericData<Comment> CreateCommentData();
    }
}