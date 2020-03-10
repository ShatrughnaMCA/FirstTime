using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NoidaAuthority.PMS.Service
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _repository;

        public CustomerService()
        {
            _repository = new CustomerRepository();
        }

        public DataSourceResult GetPaymentReceiptScheduleListById(DataSourceRequest request, PaymentViewModel model)
        {
            return _repository.GetPaymentReceiptScheduleListById(request, model);
        }

        public DataSourceResult GetPaymentScheduleDataListById(DataSourceRequest request, int? rid)
        {
            return _repository.GetPaymentScheduleDataListById(request, rid);
        }

        public DataSourceResult GetPaymentRescheduledListById(DataSourceRequest request, int? rid)
        {
            return _repository.GetPaymentRescheduledListById(request, rid);
        }

        public DataSourceResult GetPaymentLedgerDataListById(DataSourceRequest request, int? rid)
        {
            return _repository.GetPaymentLedgerDataListById(request, rid);
        }


        public DataSourceResult GetServiceHistoryDataListById(DataSourceRequest request, ServiceViewModel model)
        {
            return _repository.GetServiceHistoryDataListById(request, model);
        }

        //public int RegisterCustomerDetails(NACustomer customer, IEnumerable<HttpPostedFileBase> files)
        //{
        //    return _repository.RegisterCustomerDetails(customer, files);
        //}

        //public PropertyDetail GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber)
        //{
        //    return _repository.GetPropertyDetails(objPropertyFilter, inumber);
        //}


        public DataSourceResult GetTransferHistoryDataListById(DataSourceRequest request, int? rid)
        {
            return _repository.GetTransferHistoryDataListById(request, rid);
        }


        public DataSourceResult GetMortgageHistoryDataListById(DataSourceRequest request, int? rid)
        {
            return _repository.GetMortgageHistoryDataListById(request, rid);
        }

        public DataSourceResult GetExtensionHistoryDataListById(DataSourceRequest request, int? rid)
        {
            return _repository.GetExtensionHistoryDataListById(request, rid);
        }

        public IEnumerable<DtoLegalHistory> GetLegalHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter)
        {
            return _repository.GetLegalHistoryByRegistrationId(objPropertyFilter);
        }

        public IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter)
        {
            return _repository.GetJalDetailsPaymentHistoryByRegistrationId(objPropertyFilter);
        }


        public string GetFileUploadHtmlForService(int? departmentId, int? serviceId)
        {
            return _repository.GetFileUploadHtmlForService(departmentId, serviceId);
        }


        public PropertyViewModel GetPropertyDetailByRegistrationId(int rid)
        {
            return _repository.GetPropertyDetailByRegistrationId(rid);
        }


        public ServiceRequestViewModel GetPropertyDetailForServiceRequestById(int rid)
        {
            return _repository.GetPropertyDetailForServiceRequestById(rid);
        }


        public ServiceRequestViewModel GetServiceRequestDetailById(int? id)
        {
            return _repository.GetServiceRequestDetailById(id);
        }

        public ServiceRequestViewModel SaveServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<System.Web.HttpPostedFileBase> files)
        {
            return _repository.SaveServiceRequestDetail(model, files);
        }


        public DataSourceResult GetDirectorShareholderDataList(DataSourceRequest request)
        {
            return _repository.GetDirectorShareholderDataList(request);
        }

        public int SaveDirectorOrShareholders(string directorName, decimal? share, string shareType)
        {
            return _repository.SaveDirectorOrShareholders(directorName, share, shareType);
        }

        public int RemoveDirectorShareholderFromList(int id)
        {
            return _repository.RemoveDirectorShareholderFromList(id);
        }

        //public User GetUserDetails(string userName)
        //{
        //    return _repository.GetUserDetails(userName);
        //}

        //public UserViewModel GetLoginUserDetails(string userName)
        //{
        //    return _repository.GetLoginUserDetails(userName);
        //}

        public bool LockUser(string userName)
        {
            return _repository.LockUser(userName);
        }

        public bool ValidateUser(string userName, string password)
        {
            return _repository.ValidateUser(userName, password);
        }

        public bool ChangePassword(string userName, string email, string newPassword)
        {
            return _repository.ChangePassword(userName, email, newPassword);
        }

        public bool ChangePassword(string email, string newPassword)
        {
            return _repository.ChangePassword(email, newPassword);
        }

        //public Role GetRoleForUser(string userName)
        //{
        //    return _repository.GetRoleForUser(userName);
        //}

        public IList<DtoList> LookupPropertyType()
        {
            return _repository.LookupPropertyType();
        }


        //public UserViewModel GetCustomerDetailById(int id)
        //{
        //    return _repository.GetCustomerDetailById(id);
        //}


        public DataSourceResult GetJalPaymentDataList(DataSourceRequest request, JalViewModel model)
        {
            return _repository.GetJalPaymentDataList(request, model);
        }

        public DataSourceResult GetLitigationDataList(DataSourceRequest request, LitigationViewModel model)
        {
            return _repository.GetLitigationDataList(request, model);
        }

        public CustomerDetailViewModel GetCustomerDetails(int rid)
        {
            return _repository.GetCustomerDetails(rid);
        }


        public DataSourceResult GetScannedDocumentListById(DataSourceRequest request, DocumentViewModel model)
        {
            return _repository.GetScannedDocumentListById(request, model);
        }

        public DataSourceResult GetGeneratedDocumentListById(DataSourceRequest request, DocumentViewModel model)
        {
            return _repository.GetGeneratedDocumentListById(request, model);
        }


        public DataSourceResult GetRegistrationIdListAsDataSource(DataSourceRequest request)
        {
            return _repository.GetRegistrationIdListAsDataSource(request);
        }


        public CustomerViewModel ValidateCustomerRegistration(CustomerViewModel model)
        {
            return _repository.ValidateCustomerRegistration(model);
        }


        public Entities.User GetUserDetails(string userName)
        {
            throw new NotImplementedException();
        }

        public Entities.Role GetRoleForUser(string userName)
        {
            throw new NotImplementedException();
        }


        public DataSourceResult GetMultiplePaymentStatusList(DataSourceRequest request, PaymentViewModel model)
        {
            return _repository.GetMultiplePaymentStatusList(model,request);
        }


        public DataSourceResult GetInstallmentPaymentDues(DataSourceRequest request, PaymentViewModel model)
        {
            return _repository.GetInstallmentPaymentDues(model, request);
        }


        public DataSourceResult GetNDCDetails(DataSourceRequest request, PaymentViewModel model)
        {
            return _repository.GetNDCDetails(model, request);
        }


        public DataSourceResult GetJalPaymentPISDataList(DataSourceRequest request, JalViewModel model)
        {
            return _repository.GetJalPaymentPISDataList(request, model);
        }

        public DataSourceResult GetLitigationPISDataList(DataSourceRequest request, LitigationViewModel model)
        {
            return _repository.GetLitigationPISDataList(request, model);
        }

        public DataSourceResult GetNoticesAndRemarksAsDataSource(DataSourceRequest request, CustomerViewModel model)
        {
            return _repository.GetNoticesAndRemarksAsDataSource(request, model);
        }


        public List<DDList> GetAllBanks()
        {
            return _repository.GetAllBanks();
        }


        public List<DDList> GetAllBranchs(int bankId)
        {
            return _repository.GetAllBranchs(bankId);
        }


        public string GetAccountNumber(int bankId, int branchId)
        {
            return _repository.GetAccountNumber(bankId, branchId);
        }


        public List<DDList> GetAllAccountHead()
        {
            return _repository.GetAllAccountHead();
        }


        public List<DDList> GetAccountSubHead(int AccountHeadId)
        {
            return _repository.GetAccountSubHead(AccountHeadId);
        }


        public int SaveCreateChallan(int rId, int AccountHeadId, int AccountSubHeadId, decimal? Amount)
        {
            return _repository.SaveCreateChallan(rId, AccountHeadId, AccountSubHeadId, Amount);
        }


        //public List<PaymentViewModel> GetGeneratedChallanDetails(int rid)
        //{
        //    return _repository.GetGeneratedChallanDetails(rid);
        //}


        public DataSourceResult GetTransferHistoryByIdAsDataSource(DataSourceRequest request, TransferViewModel model)
        {
            return _repository.GetTransferHistoryByIdAsDataSource(request, model);
        }

        public DataSourceResult GetMortgageHistoryByIdAsDataSource(DataSourceRequest request, MortgageViewModel model)
        {
            return _repository.GetMortgageHistoryByIdAsDataSource(request, model);
        }

        public DataSourceResult GetExtensionHistoryByIdAsDataSource(DataSourceRequest request, ExtensionViewModel model)
        {
            return _repository.GetExtensionHistoryByIdAsDataSource(request, model);
        }

        public DataSourceResult GetRentingHistoryByIdAsDataSource(DataSourceRequest request, RentingViewModel model)
        {
            return _repository.GetRentingHistoryByIdAsDataSource(request, model);
        }

        public DataSourceResult GetCICHistoryByIdAsDataSource(DataSourceRequest request, CICViewModel model)
        {
            return _repository.GetCICHistoryByIdAsDataSource(request, model);
        }

        public DataSourceResult GetFunctionalHistoryByIdAsDataSource(DataSourceRequest request, FunctionalViewModel model)
        {
            return _repository.GetFunctionalHistoryByIdAsDataSource(request, model);
        }


        public DataSourceResult GetServiceRequestUploadedDocumentsById(DataSourceRequest request, ServiceViewModel model)
        {
            return _repository.GetServiceRequestUploadedDocumentsById(request, model);
        }


        public ServiceRequestViewModel ReSubmitServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            return _repository.ReSubmitServiceRequestDetail(model, files);
        }


        public PaymentViewModel GetNDCDetailsByRid(int rid)
        {
            return _repository.GetNDCDetailsByRid(rid);
        }


        public string GetUploadedDocumentByServiceId(int Id, int Rid, string ActionType)
        {
            return _repository.GetUploadedDocumentByServiceId(Id, Rid, ActionType);
        }
    }
}
