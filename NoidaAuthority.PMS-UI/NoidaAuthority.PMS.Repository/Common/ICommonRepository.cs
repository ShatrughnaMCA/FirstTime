using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Repository
{
    public interface ICommonRepository
    {
        IList<DtoList> LookupPropertyType();
        IList<DtoList> LookupArea();
        IList<DtoList> LookupSector(int propertyID);
        IList<DtoList> LookupBlock(DtoPropertyFilter objPropertyFilter);
        IList<DtoList> LookupAlotmentStatus();
        IList<DtoList> AutoFillPropertyNo(DtoPropertyFilter objPropertyFilter);
        IList<DtoList> AutoFillAlloteName(DtoPropertyFilter objPropertyFilter);
        IList<DtoList> AutoFillPlotNumber(DtoPropertyFilter objPropertyFilter);
        PropertyCount GetPropertyCount();
        List<DtoList> GetLegalDDL();
        List<DtoList> GetRevYears();
        List<DtoList> GetAllServiceData(int deptt);
        List<DDList> YesNoDDL();
        DtoPropertyDetails GetDepttByRID(int RID);

        List<DDList> GetDepartmentList();

        List<DDList> GetServicesList(int? departmentId);



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

        IList<DropdownViewModel> GetPropertyType();

        IList<DropdownViewModel> GetPropertyTypeII();

        IList<DropdownViewModel> GetAreaTypeII();

        IList<DropdownViewModel> GetSectorListII(int propertyId);

        IList<DropdownViewModel> GetBlockListII(PropertyFilterViewModel model);

        List<DropdownViewModel> GetServicesListII();

        Kendo.Mvc.UI.DataSourceResult GetBankDetailAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model);
    }
}
