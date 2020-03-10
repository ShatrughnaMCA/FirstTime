using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Service
{
    public class CommonService : ICommonService
    {
        ICommonRepository commonRepository;

        public CommonService()
        {
            commonRepository = new CommonRepository();

        }
        public IList<DtoList> GetPropertyType()
        {
            return commonRepository.LookupPropertyType();
        }
        public IList<DtoList> GetAreaType()
        {
            return commonRepository.LookupArea();
        }

        public IList<DtoList> GetSector(int propertyID)
        {
            return commonRepository.LookupSector(propertyID);
        }

        public List<DtoList> GetLegalDDL()
        {
            return commonRepository.GetLegalDDL();
        }

        public List<DtoList> GetRevYears()
        {
            return commonRepository.GetRevYears();
        }

        public IList<DtoList> GetBlock(DtoPropertyFilter objPropertyFilter)
        {
            return commonRepository.LookupBlock(objPropertyFilter);
        }

        public IList<DtoList> GetAlotmentStatus()
        {
            return commonRepository.LookupAlotmentStatus();
        }

        public IList<DtoList> GetPropertyNo(DtoPropertyFilter objPropertyFilter)
        {
            return commonRepository.AutoFillPropertyNo(objPropertyFilter);
        }


        public IList<DtoList> GetAlloteName(DtoPropertyFilter objPropertyFilter)
        {
            return commonRepository.AutoFillAlloteName(objPropertyFilter);
        }

        public IList<DtoList> GetPlotNumber(DtoPropertyFilter objPropertyFilter)
        {
            return commonRepository.AutoFillPlotNumber(objPropertyFilter);
        }

        public PropertyCount GetPropertyCount()
        {
            return commonRepository.GetPropertyCount();
        }
        public List<DtoList> GetAllServiceData(int deptt)
        {
            return commonRepository.GetAllServiceData(deptt);
        }

        public List<DDList> YesNoDDL()
        {
            return commonRepository.YesNoDDL();
        }

        public DtoPropertyDetails GetDepttByRID(int RID)
        {
            return commonRepository.GetDepttByRID(RID);
        }


        public List<DDList> GetDepartmentList()
        {
            return commonRepository.GetDepartmentList();
        }


        public List<DDList> GetServicesList(int? departmentId)
        {
            return commonRepository.GetServicesList(departmentId);
        }



        public List<DropdownViewModel> GetServiceListByDepartment(int departmentId)
        {
            return commonRepository.GetServiceListByDepartment(departmentId);
        }

        public List<DropdownViewModel> GetSubDepartmentList(int departmentId)
        {
            return commonRepository.GetSubDepartmentList(departmentId);
        }

        public List<DropdownViewModel> GetTransferTypeList()
        {
            return commonRepository.GetTransferTypeList();
        }

        public List<DropdownViewModel> GetTransferSubTypeList(int transferTypeId)
        {
            return commonRepository.GetTransferSubTypeList(transferTypeId);
        }


        public List<DropdownViewModel> GetCICRequestTypeList()
        {
            return commonRepository.GetCICRequestTypeList();
        }

        public List<DropdownViewModel> GetCompanyMemberTypeList()
        {
            return commonRepository.GetCompanyMemberTypeList();
        }

        public List<DropdownViewModel> GetFirmStatusList()
        {
            return commonRepository.GetFirmStatusList();
        }


        public List<DropdownViewModel> GetGenderList()
        {
            return commonRepository.GetGenderList();
        }

        public List<DropdownViewModel> GetOccupationList()
        {
            return commonRepository.GetOccupationList();
        }

        public List<DropdownViewModel> GetMortgageTypeList()
        {
            return commonRepository.GetMortgageTypeList();
        }

        public List<DropdownViewModel> GetGPAStatusList()
        {
            return commonRepository.GetGPAStatusList();
        }


        public List<DropdownViewModel> GetNOCStatusList()
        {
            return commonRepository.GetNOCStatusList();
        }


        public List<DropdownViewModel> GetDepartmentListII()
        {
            return commonRepository.GetDepartmentListII();
        }


        public List<DropdownViewModel> GetSecurityQuestionList()
        {
            return commonRepository.GetSecurityQuestionList();
        }


        public List<DropdownViewModel> GetCustomerIdList()
        {
            return commonRepository.GetCustomerIdList();
        }


        public List<DropdownViewModel> GetLetterTypeList()
        {
            return commonRepository.GetLetterTypeList();
        }


        public List<DropdownViewModel> GetRoleTypeList()
        {
            return commonRepository.GetRoleTypeList();
        }


        public List<DropdownViewModel> GetMiscellaneousTypeList(DropdownViewModel model)
        {
            return commonRepository.GetMiscellaneousTypeList(model);
        }


        public ActionViewModel SendMessageOnMobileAndEmail(ActionViewModel model)
        {
            return commonRepository.SendMessageOnMobileAndEmail(model);
        }


        public List<DropdownViewModel> GetSectorList(DropdownViewModel model)
        {
            return commonRepository.GetSectorList(model);
        }

        public List<DropdownViewModel> GetBlockList(DropdownViewModel model)
        {
            return commonRepository.GetBlockList(model);
        }

        IList<DropdownViewModel> GetPropertyTypeII()
        {
            return commonRepository.GetPropertyTypeII();
        }

        IList<DropdownViewModel> GetAreaTypeII()
        {
            return commonRepository.GetAreaTypeII();
        }

        public IList<DropdownViewModel> GetSectorListII(int propertyId)
        {
            return commonRepository.GetSectorListII(propertyId);
        }

        public IList<DropdownViewModel> GetBlockListII(PropertyFilterViewModel model)
        {
            return commonRepository.GetBlockListII(model);
        }

        //List<DropdownViewModel> GetDepartmentList()
        //{
        //    return commonRepository.GetDepartmentList();
        //}

        List<DropdownViewModel> GetServicesListII(int? departmentId)
        {
            return commonRepository.GetServicesListII();
        }

        IList<DropdownViewModel> ICommonService.GetPropertyTypeII()
        {
            throw new NotImplementedException();
        }

        IList<DropdownViewModel> ICommonService.GetAreaTypeII()
        {
            throw new NotImplementedException();
        }

        List<DropdownViewModel> ICommonService.GetDepartmentList()
        {
            throw new NotImplementedException();
        }

        List<DropdownViewModel> ICommonService.GetServicesList(int? departmentId)
        {
            throw new NotImplementedException();
        }


        public Kendo.Mvc.UI.DataSourceResult GetBankDetailAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model)
        {
            return commonRepository.GetBankDetailAsDataSource(request, model);
        }
    }
}
