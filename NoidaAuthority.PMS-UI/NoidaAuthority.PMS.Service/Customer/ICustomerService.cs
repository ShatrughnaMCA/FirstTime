using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NoidaAuthority.PMS.Service
{
    public interface ICustomerService
    {
        DataSourceResult GetPaymentReceiptScheduleListById(DataSourceRequest request, PaymentViewModel model);

        DataSourceResult GetPaymentScheduleDataListById(DataSourceRequest request, int? rid);

        DataSourceResult GetPaymentRescheduledListById(DataSourceRequest request, int? rid);

        DataSourceResult GetPaymentLedgerDataListById(DataSourceRequest request, int? rid);

        DataSourceResult GetServiceHistoryDataListById(DataSourceRequest request, ServiceViewModel model);

        DataSourceResult GetTransferHistoryDataListById(DataSourceRequest request, int? rid);

        DataSourceResult GetMortgageHistoryDataListById(DataSourceRequest request, int? rid);

        DataSourceResult GetExtensionHistoryDataListById(DataSourceRequest request, int? rid);

        string GetFileUploadHtmlForService(int? departmentId, int? serviceId);

        //int RegisterCustomerDetails(NACustomer customer, IEnumerable<HttpPostedFileBase> files);

        //PropertyDetail GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber);

        IEnumerable<DtoLegalHistory> GetLegalHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter);

        IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter);

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

        CustomerDetailViewModel GetCustomerDetails(int proid);

        DataSourceResult GetScannedDocumentListById(DataSourceRequest request, DocumentViewModel model);

        DataSourceResult GetGeneratedDocumentListById(DataSourceRequest request, DocumentViewModel model);

        DataSourceResult GetRegistrationIdListAsDataSource(DataSourceRequest request);

        CustomerViewModel ValidateCustomerRegistration(CustomerViewModel model);

        DataSourceResult GetMultiplePaymentStatusList(DataSourceRequest request, PaymentViewModel model);

        DataSourceResult GetInstallmentPaymentDues(DataSourceRequest request, PaymentViewModel model);

        DataSourceResult GetNDCDetails(DataSourceRequest request, PaymentViewModel model);

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
