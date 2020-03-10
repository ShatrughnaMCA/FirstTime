using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;


namespace NoidaAuthority.PMS.Service
{
    public class PropertyService : IPropertyService
    {
        IPropertyRepository propertyRepository;
        public PropertyService()
        {
            propertyRepository = new PropertyRepository();
        }
        public IEnumerable<DtoProperty> GetPropertyList(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPropertyList(objPropertyFilter);
        }
        public DataSourceResult GetPropertyList(DtoPropertyFilter objPropertyFilter, DataSourceRequest request)
        {
            return propertyRepository.GetPropertyList(objPropertyFilter, request);
        }

        public IEnumerable<DocumentDetail> GetPropertyAttachmentDetails(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPropertyAttachmentDetails(objPropertyFilter);
        }

        public NotificationDetails GetMobileEmailByReqId(int reqId)
        {
            return propertyRepository.GetMobileEmailByReqId(reqId);
        }

        public DtoPropertyDetails GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber)
        {
            return propertyRepository.GetPropertyDetails(objPropertyFilter, inumber);
        }
        public IEnumerable<DtoPropertyPaymentHistory> GetPropertyPaymentHistory(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPaymentHistoryByPropertyId(objPropertyFilter);
        }
        public IEnumerable<DtoPropertyPaymentHistory> GetPropertyPaymentCustomerHistory(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPaymentHistoryCustomerByPropertyId(objPropertyFilter);
        }
        public IEnumerable<DtoPropertyPaymentHistory> GetPaymentList(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPaymentList(objPropertyFilter);
        }

        public DtoSectorLocation GetSectorLocation(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetSectorLocation(objPropertyFilter);
        }
        public IEnumerable<DtoPropertyLocation> GetPropertyLocations(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPropertyLocations(objPropertyFilter);
        }
        public IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistory(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetJalDetailsPaymentHistoryByPropertyId(objPropertyFilter);
        }

        public IEnumerable<DtoTransferHistory> GetTransferHistory(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetTransferHistoryByPropertyId(objPropertyFilter);
        }

        public bool SaveOtherRequest(string desc, int reqID)
        {
            return propertyRepository.SaveOtherRequest(desc, reqID);
        }

        public IEnumerable<DtoLegalHistory> GetLegalHistory(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetLegalHistoryByPropertyId(objPropertyFilter);
        }
        public IEnumerable<DtoPaymentSchedule> GetPaymentSchedule(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPaymentScheduleByPropertyId(objPropertyFilter);
        }
        public IEnumerable<DtoPaymentDetails> GetPaymentDetails(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPaymentDetails(objPropertyFilter);
        }

        public IEnumerable<DtoDepartmentPaymentDetails> GetPropertiesByPaymentStatus(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetDepartmentPaymentDetails(objPropertyFilter);
        }
        public IEnumerable<DtoLegalwithFarmerDetails> GetLegalPropertiesofHighCourt(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetLegalwithFarmerDetails(objPropertyFilter);
        }
        public IEnumerable<DtoPropertyComplaintList> GetPropertyComplaintList(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPropertyComplaintList(objPropertyFilter);
        }
        public IEnumerable<DtoPropertyComplaintList> GetPropertyComplaintListByRegistrationId(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPropertyComplaintListByRegistrationId(objPropertyFilter);
        }
        public DtoSchedulePaymentSummary GetPaymentScheduleSummary(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.GetPaymentScheduleSummaryByPropertyId(objPropertyFilter);
        }
        public DtoPaymentPayStatus UpdatePaymentPayStatus(DtoPropertyFilter objPropertyFilter)
        {
            return propertyRepository.UpdatePaymentPayStatus(objPropertyFilter);
        }
        public DtoPropertyDetails GetPropertyDetailsByRequestId(int RequestId)
        {
            return propertyRepository.GetPropertyDetailsByRequestId(RequestId);
        }

        public IEnumerable<PropertyDocument> GetPropertyDocument(int resigtrationID)
        {
            IList<PropertyDocument> documentList = new List<PropertyDocument>();
            documentList = GetFiles(resigtrationID);
            return documentList;
        }
        private List<PropertyDocument> GetFiles(int registrationID)
        {
            List<PropertyDocument> lstFiles = new List<PropertyDocument>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath(string.Format("~/Documents/{0}", registrationID)));
            int i = 1;
            if (dirInfo.Exists)
            {
                foreach (var item in dirInfo.GetFiles())
                {
                    lstFiles.Add(new PropertyDocument()
                    {
                        ID = i.ToString(),
                        RegistrationId = registrationID,
                        Name = GetDocumentDisplayName(Path.GetFileNameWithoutExtension(item.Name)),
                        DocumentType = item.CreationTime.ToString(),
                        Url = string.Format("/Documents/{0}/{1}", registrationID, item.Name),
                    }
                    );
                    i++;
                }
            }
            return lstFiles;
        }
        private string GetDocumentDisplayName(string name)
        {
            string strDisplayName = string.Empty;
            switch (name)
            {
                case "BuildingPlan":
                    strDisplayName = "Building Plan";
                    break;
                case "FunctionalCertificate":
                    strDisplayName = "Functional Certificate";
                    break;
                case "InitialPlan":
                    strDisplayName = "Initial Plan";
                    break;
                case "LeaseDeed":
                    strDisplayName = "Lease Deed";
                    break;
                default:
                    strDisplayName = name;
                    break;
            }
            return strDisplayName;
        }

        public bool AddPropertyRemark(string registrationId, string remark)
        {
            return propertyRepository.AddPropertyRemark(registrationId, remark);
        }

        public List<RemarksModel> Remarks_Read(string id)
        {
            return propertyRepository.Remarks_Read(id);
        }

        //public List<PropertyRemark> GetPropertyRemark(string registrationId)
        //{
        //    return propertyRepository.GetPropertyRemark(registrationId);
        //}

        public DataSourceResult GetPropertyListByID(DtoPropertyFilter objPropertyFilter, DataSourceRequest request)
        {
            return propertyRepository.GetPropertyListByID(objPropertyFilter, request);
        }

        public bool SaveFeedback(string feedback, string regNo)
        {
            return propertyRepository.SaveFeedback(feedback, regNo);
        }

        public DataSourceResult Feedbacks_Read(DataSourceRequest request)
        {
            return propertyRepository.Feedbacks_Read(request);
        }

        public bool SaveServiceReq(int deptVal, int serviceVal, string description, string regNo)
        {
            return propertyRepository.SaveServiceReq(deptVal, serviceVal, description, regNo);
        }

        public DataSourceResult SR_Read(DataSourceRequest request)
        {
            return propertyRepository.SR_Read(request);
        }


        public NoidaServiceRequestModel GetPropertyDetailForServiceRequest(int id)
        {
            return propertyRepository.GetPropertyDetailForServiceRequest(id);
        }

        public DataSourceResult GetCitizenRequests(DataSourceRequest req)
        {
            return propertyRepository.GetCitizenRequests(req);
        }

        public RequestDetails GetRequestDetailsById(int id)
        {
            return propertyRepository.GetRequestDetailsById(id);
        }

        //public DataSourceResult GetDocumentDetails(DataSourceRequest req, int rid, int id)
        //{            
        //    return propertyRepository.GetDocumentDetails(req, rid, id);
        //}

        //public bool SaveRequestDetails(int id, decimal? serviceFee, decimal? duesAmnt, string comment, int temp)
        //{
        //    return propertyRepository.SaveRequestDetails(id, serviceFee, duesAmnt, comment, temp);
        //}

        public List<CitizenPropertyDocument> GetDocumentDetails(DataSourceRequest req, int rid, int id)
        {
            NoidaAuthority.PMS.Repository.PropertyRepository prepo = new PropertyRepository();
            return prepo.GetDocumentDetails(req, rid, id);
        }
        public List<DDList> GetMortgageType()
        {
            return propertyRepository.GetMortgageType();
        }
        public List<DDList> GetMortgagePrevLoan()
        {
            return propertyRepository.GetMortgagePrevLoan();
        }
        public bool SaveMortgageReq(int rid, int registrationno, string BankName, string BranchAddress, decimal SanctionedAmount, string MortgageType, short PreviousLoanNoc, string desc)
        {
            return propertyRepository.SaveMortgageReq(rid, registrationno, BankName, BranchAddress, SanctionedAmount, MortgageType, PreviousLoanNoc, desc);
        }

        public bool SaveTransferRequest(int transType, int transSubType, string firstName, string middleName, string lastName, string relativeName, string motherName, string mobileNo, string email, string correspondenceAdd, string permanentAdd, string PAN, int? occupation, int rId, int serviceId, string gender, string cmpnyName, string signAuth, string RO, int reqID, string desc)
        {
            return propertyRepository.SaveTransferRequest(transType, transSubType, firstName, middleName, lastName, relativeName, motherName, mobileNo, email, correspondenceAdd, permanentAdd, PAN, occupation, rId, serviceId, gender, cmpnyName, signAuth, RO, reqID, desc);
        }

        public List<DDList> GetOccupation()
        {
            return propertyRepository.GetOccupation();
        }

        public List<DDList> GetGender()
        {
            return propertyRepository.GetGender();
        }

        public List<DDList> GetTransferTypes()
        {
            return propertyRepository.GetTransferTypes();
        }

        public List<DDList> GetTransferSubTypes(int TransferType)
        {
            return propertyRepository.GetTransferSubTypes(TransferType);
        }

        public List<DDList> BindDDL(string type)
        {
            return propertyRepository.BindDDL(type);
        }

        public DataSourceResult GetDirectorDetails(DataSourceRequest req, int rid)
        {
            return propertyRepository.GetDirectorDetails(req, rid);
        }

        // To Save CIC
        public bool SaveCIC(int rid, int registrationno, decimal directorShare, int changeType, decimal cicCharge, string directorName, string desc, int type)
        {
            return propertyRepository.SaveCIC(rid, registrationno, directorShare, changeType, cicCharge, directorName, desc, type);
        }

        public int SaveCICFirmName(int rid, int registrationno, int newFirmStatus, string newFirmName, string desc, decimal ciccharge)
        {
            return propertyRepository.SaveCICFirmName(rid, registrationno, newFirmStatus, newFirmName, desc, ciccharge);
        }

        public int SaveForFirmProduct(int rid, int registrationno, string newFirmProduct, string desc, decimal ciccharge = 0)
        {
            return propertyRepository.SaveForFirmProduct(rid, registrationno, newFirmProduct, desc, ciccharge = 0);
        }

        //To Remove Record (Soft Delete)
        public bool RemoveRecord(int dirID)
        {
            return propertyRepository.RemoveRecord(dirID);
        }

        public bool SaveMutationRequest(DateTime transDeedDate, string bahiNo, string bahiZildNo, string bahiPgNo, string SINo, int rId, int serviceId, string desc)
        {
            return propertyRepository.SaveMutationRequest(transDeedDate, bahiNo, bahiZildNo, bahiPgNo, SINo, rId, serviceId, desc);
        }


        public bool SaveRentServiceReq(int rid, int serviceId, string tenantName, string tenantProject, decimal rentingCharge, DateTime rentingDate, int rentDuration, string desc)
        {
            return propertyRepository.SaveRentServiceReq(rid, serviceId, tenantName, tenantProject, rentingCharge, rentingDate, rentDuration, desc);
        }

        public bool SavegpaRequest(GPAModel objgpaModel)
        {
            return propertyRepository.SavegpaRequest(objgpaModel);
        }


        public object DownloadChallan(int challanId)
        {
            return propertyRepository.DownloadChallan(challanId);
        }


        public int SaveExtensionRequest(int rid, int requestId, DateTime extensionDueDate, DateTime extensionGivenDate, string desc)
        {
            return propertyRepository.SaveExtensionRequest(rid, requestId, extensionDueDate, extensionGivenDate, desc);
        }


        public DataSourceResult GetPropertyDetailsForExtension(DataSourceRequest request, int departmentId, int ServiceId)
        {
            return propertyRepository.GetPropertyDetailsForExtension(request, departmentId, ServiceId);
        }


        public DataSourceResult GetServiceRequestReport(DataSourceRequest request)
        {
            return propertyRepository.GetServiceRequestReport(request);
        }


        public DataSourceResult GetReportingService(DataSourceRequest request)
        {
            return propertyRepository.GetReportingService(request);
        }


        public DataSourceResult GetServiceRequestReport(DataSourceRequest request, int? departmentId, DateTime? fromDate, DateTime? toDate, int? serviceId, string registrationId)
        {
            return propertyRepository.GetServiceRequestReport(request, departmentId, fromDate, toDate, serviceId, registrationId);
        }

        public DataSourceResult GetPropertySearchList(DtoPropertyFilter objPropertyFilter, DataSourceRequest request)
        {
            return propertyRepository.GetPropertySearchList(objPropertyFilter, request);
        }


        public DataSourceResult GetServiceRequestForUnRegisteredProperty(DataSourceRequest request)
        {
            return propertyRepository.GetServiceRequestForUnRegisteredProperty(request);
        }
    }
}
