using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;


namespace NoidaAuthority.PMS.Repository
{
    public interface IPropertyRepository
    {
        IEnumerable<DtoProperty> GetPropertyList(DtoPropertyFilter objPropertyFilter);
        DataSourceResult GetPropertyList(DtoPropertyFilter objPropertyFilter, DataSourceRequest request);
        DtoPropertyDetails GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber);
        IEnumerable<DtoPropertyPaymentHistory> GetPaymentHistoryByPropertyId(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPropertyPaymentHistory> GetPaymentList(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistoryByPropertyId(DtoPropertyFilter objPropertyFilter);
        //// For Sector and Property Locations
        DtoSectorLocation GetSectorLocation(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPropertyLocation> GetPropertyLocations(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoTransferHistory> GetTransferHistoryByPropertyId(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoLegalHistory> GetLegalHistoryByPropertyId(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPaymentSchedule> GetPaymentScheduleByPropertyId(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPaymentDetails> GetPaymentDetails(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoDepartmentPaymentDetails> GetDepartmentPaymentDetails(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoLegalwithFarmerDetails> GetLegalwithFarmerDetails(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPropertyComplaintList> GetPropertyComplaintList(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPropertyComplaintList> GetPropertyComplaintListByRegistrationId(DtoPropertyFilter objPropertyFilter);
        DtoSchedulePaymentSummary GetPaymentScheduleSummaryByPropertyId(DtoPropertyFilter objPropertyFilter);
        DtoPaymentPayStatus UpdatePaymentPayStatus(DtoPropertyFilter objPropertyFilter);
        IEnumerable<DtoPropertyPaymentHistory> GetPaymentHistoryCustomerByPropertyId(DtoPropertyFilter objPropertyFilter);
        DataSourceResult GetPropertyListByID(DtoPropertyFilter objPropertyFilter, DataSourceRequest request);
        IEnumerable<DocumentDetail> GetPropertyAttachmentDetails(DtoPropertyFilter objPropertyFilter);
        NotificationDetails GetMobileEmailByReqId(int reqId);
        bool AddPropertyRemark(string registrationId, string remark);
        List<RemarksModel> Remarks_Read(string id);
        //List<PropertyRemark> GetPropertyRemark(string registrationId);
        bool SaveFeedback(string feedback, string regNo);
        DataSourceResult Feedbacks_Read(DataSourceRequest request);
        bool SaveServiceReq(int deptVal, int serviceVal, string description, string regNo);
        DataSourceResult SR_Read(DataSourceRequest request);
        bool SaveOtherRequest(string desc, int reqID);
        NoidaServiceRequestModel GetPropertyDetailForServiceRequest(int id);
        DataSourceResult GetCitizenRequests(DataSourceRequest req);
        RequestDetails GetRequestDetailsById(int id);
        List<CitizenPropertyDocument> GetDocumentDetails(DataSourceRequest req, int rid, int id);
        //bool SaveRequestDetails(int id, decimal? serviceFee, decimal? duesAmnt, string comment, int temp);
        DtoPropertyDetails GetPropertyDetailsByRequestId(int RequestId);
        List<DDList> GetMortgageType();
        List<DDList> GetMortgagePrevLoan();
        bool SaveTransferRequest(int transType, int transSubType, string firstName, string middleName, string lastName, string relativeName, string motherName, string mobileNo, string email, string correspondenceAdd, string permanentAdd, string PAN, int? occupation, int rId, int serviceId, string gender, string cmpnyName, string signAuth, string RO, int reqID, string desc);
        List<DDList> GetOccupation();
        List<DDList> GetGender();
        List<DDList> GetTransferTypes();
        List<DDList> GetTransferSubTypes(int TransferType);
        bool SaveMortgageReq(int rid, int registrationno, string BankName, string BranchAddress, decimal SanctionedAmount, string MortgageType, short PreviousLoanNoc, string desc);
        List<DDList> BindDDL(string type);
        DataSourceResult GetDirectorDetails(DataSourceRequest req, int rid);
        // To Save CIC
        bool SaveCIC(int rid, int registrationno, decimal directorShare, int changeType, decimal cicCharge, string directorName, string desc, int type);

        int SaveCICFirmName(int rid, int registrationno, int newFirmStatus, string newFirmName, string desc, decimal ciccharge);

        int SaveForFirmProduct(int rid, int registrationno, string newFirmProduct, string desc, decimal ciccharge = 0);

        //To Remove Record (Soft Delete)
        bool RemoveRecord(int dirID);
        bool SaveMutationRequest(DateTime transDeedDate, string bahiNo, string bahiZildNo, string bahiPgNo, string SINo, int rId, int serviceId, string desc);

        bool SaveRentServiceReq(int rid, int serviceId, string tenantName, string tenantProject, decimal rentingCharge, DateTime rentingDate, int rentDuration, string desc);
        bool SavegpaRequest(GPAModel objgpaModel);

        object DownloadChallan(int challanId);

        int SaveExtensionRequest(int rid, int requestId, DateTime extensionDueDate, DateTime extensionGivenDate, string desc);

        DataSourceResult GetPropertyDetailsForExtension(DataSourceRequest request, int departmentId, int ServiceId);

        DataSourceResult GetServiceRequestReport(DataSourceRequest request);

        DataSourceResult GetReportingService(DataSourceRequest request);

        DataSourceResult GetServiceRequestReport(DataSourceRequest request, int? departmentId, DateTime? fromDate, DateTime? toDate, int? serviceId, string registrationId);
        DataSourceResult GetPropertySearchList(DtoPropertyFilter objPropertyFilter, DataSourceRequest request);

        DataSourceResult GetServiceRequestForUnRegisteredProperty(DataSourceRequest request);
    }
}
