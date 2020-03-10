using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using System.Collections.Generic;

namespace NoidaAuthority.PMS.Service
{
    public interface ICommonService
    {
        IList<DropdownViewModel> GetPropertyTypeII();
        IList<DropdownViewModel> GetAreaTypeII();
        IList<DropdownViewModel> GetSectorListII(int propertyId);
        IList<DropdownViewModel> GetBlockListII(PropertyFilterViewModel model);
        List<DropdownViewModel> GetDepartmentList();
        List<DropdownViewModel> GetServicesList(int? departmentId);

        IList<DtoList> GetAlotmentStatus();
        IList<DtoList> GetPropertyNo(DtoPropertyFilter objPropertyFilter);
        IList<DtoList> GetAlloteName(DtoPropertyFilter objPropertyFilter);
        IList<DtoList> GetPlotNumber(DtoPropertyFilter objPropertyFilter);        
        List<DtoList> GetLegalDDL();
        List<DtoList> GetRevYears();
        List<DtoList> GetAllServiceData(int deptt);
        List<DDList> YesNoDDL();
        
        PropertyCount GetPropertyCount();
        DtoPropertyDetails GetDepttByRID(int RID);



        IList<DtoList> GetPropertyType();
        IList<DtoList> GetAreaType();
        IList<DtoList> GetSector(int propertyID);
        IList<DtoList> GetBlock(DtoPropertyFilter objPropertyFilter);
        //IList<DtoList> GetAlotmentStatus();
        //IList<DtoList> GetPropertyNo(DtoPropertyFilter objPropertyFilter);
        //IList<DtoList> GetAlloteName(DtoPropertyFilter objPropertyFilter);
        //IList<DtoList> GetPlotNumber(DtoPropertyFilter objPropertyFilter);
        //PropertyCount GetPropertyCount();
        //List<DtoList> GetLegalDDL();
        //List<DtoList> GetRevYears();
        //List<DtoList> GetAllServiceData(int deptt);
        //List<DDList> YesNoDDL();
        //DtoPropertyDetails GetDepttByRID(int RID);
        //List<DDList> GetDepartmentList();
        //List<DDList> GetServicesList(int? departmentId);

        List<DropdownViewModel> GetServiceListByDepartment(int departmentId);

        List<DropdownViewModel> GetSubDepartmentList(int departmentId);

        List<DropdownViewModel> GetTransferTypeList();

        List<DropdownViewModel> GetTransferSubTypeList(int transferTypeId);

        List<DropdownViewModel> GetCICRequestTypeList();

        List<DropdownViewModel> GetCompanyMemberTypeList();

        List<DropdownViewModel> GetFirmStatusList();

        List<DropdownViewModel> GetGenderList();

        List<DropdownViewModel> GetOccupationList();

        List<DropdownViewModel> GetMortgageTypeList();

        List<DropdownViewModel> GetGPAStatusList();

        List<DropdownViewModel> GetNOCStatusList();

        List<DropdownViewModel> GetDepartmentListII();

        List<DropdownViewModel> GetSecurityQuestionList();

        List<DropdownViewModel> GetCustomerIdList();

        List<DropdownViewModel> GetLetterTypeList();

        List<DropdownViewModel> GetRoleTypeList();

        List<DropdownViewModel> GetMiscellaneousTypeList(DropdownViewModel model);


        ActionViewModel SendMessageOnMobileAndEmail(ActionViewModel model);

        List<DropdownViewModel> GetSectorList(DropdownViewModel model);

        List<DropdownViewModel> GetBlockList(DropdownViewModel model);

        DataSourceResult GetBankDetailAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model);
    }
}
