using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NoidaAuthority.PMS.Repository
{
    public interface ICustomerRepository
    {
        DataSourceResult GetPaymentReceiptScheduleListById(DataSourceRequest request, PaymentViewModel model);

        DataSourceResult GetPaymentScheduleDataListById(DataSourceRequest request, int? id);

        DataSourceResult GetPaymentRescheduledListById(DataSourceRequest request, int? id);

        DataSourceResult GetPaymentLedgerDataListById(DataSourceRequest request, int? id);

        DataSourceResult GetServiceHistoryDataListById(DataSourceRequest request, ServiceViewModel model);

        //int RegisterCustomerDetails(NACustomer customer, IEnumerable<HttpPostedFileBase> files);

        //PropertyDetail GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber);

        DataSourceResult GetTransferHistoryDataListById(DataSourceRequest request, int? rid);

        DataSourceResult GetMortgageHistoryDataListById(DataSourceRequest request, int? rid);

        DataSourceResult GetExtensionHistoryDataListById(DataSourceRequest request, int? rid);

        IEnumerable<DtoLegalHistory> GetLegalHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter);

        IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter);

        string GetFileUploadHtmlForService(int? departmentId, int? serviceId);

        PropertyViewModel GetPropertyDetailByRegistrationId(int rid);

        ServiceRequestViewModel GetPropertyDetailForServiceRequestById(int rid);

        ServiceRequestViewModel GetServiceRequestDetailById(int? id);

        ServiceRequestViewModel SaveServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<System.Web.HttpPostedFileBase> files);

        DataSourceResult GetDirectorShareholderDataList(DataSourceRequest request);

        int SaveDirectorOrShareholders(string directorName, decimal? share, string shareType);

        int RemoveDirectorShareholderFromList(int id);

        //User GetUserDetails(string userName);

        //UserViewModel GetLoginUserDetails(string userName);

        bool LockUser(string userName);

        bool ValidateUser(string userName, string password);

        bool ChangePassword(string userName, string email, string newPassword);

        bool ChangePassword(string email, string newPassword);

        //Role GetRoleForUser(string userName);

        IList<DtoList> LookupPropertyType();

        //UserViewModel GetCustomerDetailById(int id);

        DataSourceResult GetJalPaymentDataList(DataSourceRequest request, JalViewModel model);

        DataSourceResult GetLitigationDataList(DataSourceRequest request, LitigationViewModel model);

        CustomerDetailViewModel GetCustomerDetails(int rid);

        DataSourceResult GetScannedDocumentListById(DataSourceRequest request, DocumentViewModel model);

        DataSourceResult GetGeneratedDocumentListById(DataSourceRequest request, DocumentViewModel model);

        DataSourceResult GetRegistrationIdListAsDataSource(DataSourceRequest request);

        CustomerViewModel ValidateCustomerRegistration(CustomerViewModel model);

        DataSourceResult GetMultiplePaymentStatusList(PaymentViewModel model,DataSourceRequest request);

        DataSourceResult GetInstallmentPaymentDues(PaymentViewModel model, DataSourceRequest request);

        DataSourceResult GetNDCDetails(PaymentViewModel model, DataSourceRequest request);

        DataSourceResult GetJalPaymentPISDataList(DataSourceRequest request, JalViewModel model);

        DataSourceResult GetLitigationPISDataList(DataSourceRequest request, LitigationViewModel model);

        DataSourceResult GetNoticesAndRemarksAsDataSource(DataSourceRequest request, CustomerViewModel model);

        List<DDList> GetAllBanks();

        List<DDList> GetAllBranchs(int bankId);

        string GetAccountNumber(int bankId, int branchId);

        List<DDList> GetAllAccountHead();

        List<DDList> GetAccountSubHead(int AccountHeadId);

        int SaveCreateChallan(int rId, int AccountHeadId, int AccountSubHeadId, decimal? Amount);

        //List<PaymentViewModel> GetGeneratedChallanDetails(int rid);

        DataSourceResult GetTransferHistoryByIdAsDataSource(DataSourceRequest request, TransferViewModel model);

        DataSourceResult GetMortgageHistoryByIdAsDataSource(DataSourceRequest request, MortgageViewModel model);

        DataSourceResult GetExtensionHistoryByIdAsDataSource(DataSourceRequest request, ExtensionViewModel model);

        DataSourceResult GetRentingHistoryByIdAsDataSource(DataSourceRequest request, RentingViewModel model);

        DataSourceResult GetCICHistoryByIdAsDataSource(DataSourceRequest request, CICViewModel model);

        DataSourceResult GetFunctionalHistoryByIdAsDataSource(DataSourceRequest request, FunctionalViewModel model);

        DataSourceResult GetServiceRequestUploadedDocumentsById(DataSourceRequest request, ServiceViewModel model);

        ServiceRequestViewModel ReSubmitServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files);

        PaymentViewModel GetNDCDetailsByRid(int rid);

        string GetUploadedDocumentByServiceId(int Id, int Rid, string ActionType);
    }
}
