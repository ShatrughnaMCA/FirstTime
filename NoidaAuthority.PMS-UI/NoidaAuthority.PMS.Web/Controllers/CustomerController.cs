using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NoidaAuthority.PMS.Web.Helper;
using System.Net;
using System.IO;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Web.Helper;
using System.Web.Routing;

using Kendo.Mvc.Extensions;
using System.IO;
using System.Net;
using System.Configuration;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Common.Helpers;

namespace NoidaAuthority.PMS.Web.Controllers
{
    public class CustomerController : WebBaseController
    {

        IPropertyService _propertyService;
        DtoPropertyFilter PropertyFilterObj;
        IManageCitizenService _manageCitizenService;
        CommonService _commonService;
        const string CHANGEDDLTYPE = "CIC";
        const string TYPE = "Director";
        const string FIRMSTATUS = "Firm Status";
        private PropertyDetail GetCitizenPropertyDetail(int id, string optString)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            _commonService = new CommonService();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
            PropertyDetail propertyDetail = new PropertyDetail();
            if (propDetails != null)
            {
                propertyDetail = SetPropertyDetailObject(propDetails);
            }

            return propertyDetail;
        }
        //
        // GET: /Customer/
        public ActionResult Index(int id, string optString)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
            PropertyDetail propertyDetail = SetPropertyDetailObject(propDetails);
            // ViewBag.Remarks =  _propertyService.GetPropertyRemark(Convert.ToString(id));
            return View(propertyDetail);
            //return View();
        }

        // GET: /Property/
        public ActionResult PropertyDetail(int id)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
            PropertyDetail propertyDetail = SetPropertyDetailObject(propDetails);

            return View(propertyDetail);
        }

        public JsonResult GetPaymentList()
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilter();
            IEnumerable<DtoPropertyPaymentHistory> propertyPaymentHistory = _propertyService.GetPaymentList(PropertyFilterObj);
            return Json(propertyPaymentHistory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPropertyPaymentHistory(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoPropertyPaymentHistory> propertyPaymentHistory = _propertyService.GetPropertyPaymentCustomerHistory(PropertyFilterObj);

            return Json(propertyPaymentHistory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJalDetailsPaymentHistory(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoJalDetailsPaymentHistory> propertyJalPaymentHistory = _propertyService.GetJalDetailsPaymentHistory(PropertyFilterObj);
            return Json(propertyJalPaymentHistory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentSchedule(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoPaymentSchedule> propertyPaymentSchedule = _propertyService.GetPaymentSchedule(PropertyFilterObj);
            return Json(propertyPaymentSchedule, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ServiceDetail()
        {
            CitizenServiceRequest model = new CitizenServiceRequest();
            return View(model);
        }
        [AllowAnonymous]
        public JsonResult GetAllServiceRequestId([DataSourceRequest] DataSourceRequest request)
        {
            _manageCitizenService = new ManageCitizenService();
            var data = _manageCitizenService.GetAllServiceRequestId(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetServiceRequestDetailByRequestId(int? requestId, string mobile)
        {
            //CitizenServiceRequest data = new CitizenServiceRequest();
            _manageCitizenService = new ManageCitizenService();
            var data = _manageCitizenService.GetServiceRequestDetailByRequestId(requestId, mobile);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AuthorityLetter()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetLetterByBarcode(string barcode)
        {
            _manageCitizenService = new ManageCitizenService();
            var data = _manageCitizenService.GetLetterByBarcode(barcode);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not", JsonRequestBehavior.AllowGet);
            }
        }

        private DtoPropertyFilter CreatePropertyFilter()
        {
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            if (Request.QueryString["pt"] != null)
                PropertyFilterObj.PropertyTypeId = Request.QueryString["pt"];
            if (Request.QueryString["pa"] != null)
                PropertyFilterObj.AreaId = Convert.ToInt32(Request.QueryString["pa"]);
            if (Request.QueryString["ps"] != null)
                PropertyFilterObj.AllotmentStatusId = Convert.ToInt32(Request.QueryString["ps"]);
            if (Request.QueryString["yr"] != null)
                PropertyFilterObj.Year = Convert.ToInt32(Request.QueryString["yr"]);
            if (Request.QueryString["sc"] != null && !string.IsNullOrEmpty(Request.QueryString["sc"]))
                PropertyFilterObj.Sector = Request.QueryString["sc"];
            if (Request.QueryString["bk"] != null)
                PropertyFilterObj.Block = Request.QueryString["bk"];
            if (Request.QueryString["pn"] != null)
                PropertyFilterObj.PropertyNo = Request.QueryString["pn"];
            if (Request.QueryString["an"] != null)
                PropertyFilterObj.AlloteName = Request.QueryString["an"];
            if (Request.QueryString["pln"] != null)
                PropertyFilterObj.PlotNumber = Request.QueryString["pln"];
            if (Request.QueryString["st"] != null)
                PropertyFilterObj.SearchType = GetSearchType(Request.QueryString["st"]);
            if (Request.QueryString["id"] != null)
            {
                int _RegistrationId = 0;
                if (int.TryParse(Request.QueryString["id"], out _RegistrationId))
                    PropertyFilterObj.RegistrationId = _RegistrationId;
            }

            return PropertyFilterObj;
        }

        private string GetSearchType(string name)
        {
            string searchType = string.Empty;
            switch (name)
            {
                case "1":
                    searchType = Contants.SearchType.Property;
                    break;
                case "2":
                    searchType = Contants.SearchType.Payment;
                    break;
                case "3":
                    searchType = Contants.SearchType.Legal;
                    break;
            }
            return searchType;
        }

        private PropertyDetail SetPropertyDetailObject(DtoPropertyDetails propDetails)
        {
            PropertyDetail propertyDetail = new PropertyDetail();

            propertyDetail.PropertyId = propDetails.PropertyId;
            propertyDetail.Name = propDetails.Name;
            propertyDetail.RegistrationNo = propDetails.RegistrationNo;
            propertyDetail.PropertyType = propDetails.PropertyType;
            propertyDetail.LegalPreceedings = propDetails.LegalPreceedings;
            propertyDetail.PaymentSchedule = propDetails.PaymentSchedule;
            propertyDetail.Sector = propDetails.Sector;
            propertyDetail.Status = propDetails.Status;
            propertyDetail.Block = propDetails.Block;
            propertyDetail.Scheme = propDetails.Scheme;
            propertyDetail.Area = propDetails.Area;
            propertyDetail.NoDuesCertificateIssued = propDetails.NoDuesCertificateIssued;
            propertyDetail.RatePerSqMtr = propDetails.RatePerSqMtr;
            propertyDetail.BuildingPlanApproved = propDetails.BuildingPlanApproved;
            propertyDetail.ConstructionCompleted = propDetails.ConstructionCompleted;
            propertyDetail.FloorAreaRatio = propDetails.FloorAreaRatio;
            propertyDetail.RegistrationId = propDetails.RegistrationId;
            propertyDetail.Allotment_Doc = propDetails.Allotment_Doc;
            propertyDetail.BuildingPlan_Doc = propDetails.BuildingPlan_Doc;
            propertyDetail.Leasedeed_Doc = propDetails.Leasedeed_Doc;
            propertyDetail.No_Dues_Doc = propDetails.No_Dues_Doc;
            propertyDetail.ConstructionPeriodAllowed = propDetails.ConstructionPeriodAllowed;

            if (!string.IsNullOrEmpty(propDetails.FunctionalDate))
            {
                propertyDetail.FunctionalDate = Convert.ToDateTime(propDetails.FunctionalDate).ToString("dd MMM yyyy");
            }
            if (Convert.ToDateTime(propDetails.LastPaymentDate).ToShortDateString() != "1/1/0001")
            {
                propertyDetail.LastPaymentDate = Convert.ToDateTime(propDetails.LastPaymentDate).ToString("dd MMM yyyy");
            }
            else
            {
                propertyDetail.LastPaymentDate = "NA";
            }
            if (Convert.ToDateTime(propDetails.NextPaymentDueDate).ToShortDateString() != "1/1/0001")
            {
                propertyDetail.NextPaymentDueDate = Convert.ToDateTime(propDetails.NextPaymentDueDate).ToString("dd MMM yyyy");
            }
            else
            {
                propertyDetail.NextPaymentDueDate = "NA";
            }
            propertyDetail.AlottmentDate = Convert.ToDateTime(propDetails.AlottmentDate).ToString("dd MMM yyyy");
            propertyDetail.RegistryDate = Convert.ToDateTime(propDetails.ResistryDate).ToString("dd MMM yyyy"); ;
            propertyDetail.RentPermission = propDetails.RentPermission;

            if (Convert.ToDateTime(propDetails.PossessionDate).ToShortDateString() != "1/1/0001")
            {
                propertyDetail.PossessionDate = Convert.ToDateTime(propDetails.PossessionDate).ToString("dd MMM yyyy");
            }
            else
            {
                propertyDetail.PossessionDate = "";
            }

            propertyDetail.MortgagePermission = propDetails.MortgagePermission;
            propertyDetail.PaymentDue = propDetails.PaymentDue;
            // propertyDetail.LegalPreceedings = propDetails.LegalPreceedings;
            //propertyDetail.Location = propDetails.Location;
            propertyDetail.CustomerAddress = propDetails.CustomerAddress;
            propertyDetail.EmailId = propDetails.EmailId;
            propertyDetail.PhoneNo = propDetails.PhoneNo;
            propertyDetail.Mobile = propDetails.Mobile;
            //propertyDetail.CoApplicantName = propDetails.CoApplicantName;
            //propertyDetail.ApplicantRelationShip = propDetails.ApplicantRelationShip;
            //propertyDetail.CoApplicantAddress = propDetails.CoApplicantAddress;
            propertyDetail.PlotNumber = propDetails.PlotNumber;
            propertyDetail.PropertyNumber = propDetails.PropertyNumber;
            propertyDetail.PropertyCategory = propDetails.PropertyCategory;
            propertyDetail.TransferCase = propDetails.TransferCase;
            propertyDetail.ExtensionGiven = propDetails.ExtensionGiven;
            propertyDetail.JalConnection = propDetails.JalConnection;

            return propertyDetail;
        }

        public void SetScheduledPayment(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            //int _RegistrationId = 0;
            //if (int.TryParse(Convert.ToString(id), out _RegistrationId))
            PropertyFilterObj.PaymentScheduleId = id;
            DtoPaymentPayStatus propertyPaymentSchedule = _propertyService.UpdatePaymentPayStatus(PropertyFilterObj);
            //return Json(propertyPaymentSchedule, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveFeedback(string feedback, string regNo)
        {
            _propertyService = new PropertyService();
            var rslt = _propertyService.SaveFeedback(feedback, regNo);
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Feedbacks_Read([DataSourceRequest] DataSourceRequest request)
        {
            _propertyService = new PropertyService();
            var rsltLst = _propertyService.Feedbacks_Read(request);
            return Json(rsltLst, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public JsonResult SaveServiceReq(string deptVal, string serviceVal, string regNo, string propertyNo, string type)
        {
            // SaveServiceReqTest();

            CitizenServiceRequest ObjCitizenServiceRequest = new CitizenServiceRequest();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            ObjCitizenServiceRequest.Registration_No = regNo;
            ObjCitizenServiceRequest.Property_No = propertyNo;
            ObjCitizenServiceRequest.DepartmentId = Convert.ToInt32(deptVal);
            ObjCitizenServiceRequest.ServiceId = Convert.ToInt32(serviceVal);
            //ObjCitizenServiceRequest.Created_By = currentUser.;
            ObjCitizenServiceRequest.Request_Status = 8;
            //ObjCitizenServiceRequest.Description = description;
            ObjCitizenServiceRequest.LastDateofPayment = System.DateTime.Now;
            ObjCitizenServiceRequest.ReqType = type;
            ObjCitizenServiceRequest.Mobile = currentUser.MobileNo;
            ObjCitizenServiceRequest.Email = currentUser.UserEmail;
            //ObjCitizenServiceRequest.Comment = "Comment";

            //var objLst = new List<ServiceRequestDocument>();
            //ServiceRequestDocument obj = new ServiceRequestDocument();
            //obj.ChkDocumentId = 1;
            //obj.DocumentPath = "Document1";
            //obj.Uploaded_By = 1;
            //obj.Uploaded_Date = System.DateTime.Now;
            //objLst.Add(obj);

            //ObjCitizenServiceRequest.DocumentList = objLst;
            ManageCitizenService objManageCitizenService = new ManageCitizenService();
            var Result = objManageCitizenService.SaveServiceRequest(ObjCitizenServiceRequest);
            return Json(Result.ServiceRequestId, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("CitizenService", "Customer", new { id = Result.ServiceRequestId });
            //return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInternalServiceRequest(string deptVal, string subDeptVal, string serviceVal, string regNo, string propertyNo, string mobile, string email, string name, string reqName, string custAddress, string reqAddress, string type)
        {
            CitizenServiceRequest serviceRequest = new CitizenServiceRequest();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            serviceRequest.Registration_No = regNo;
            serviceRequest.Name = name;
            serviceRequest.RequesterName = reqName;
            serviceRequest.ApplicantAddress = custAddress;
            serviceRequest.RequestorAddress = reqAddress;
            serviceRequest.DepartmentId = Convert.ToInt32(deptVal);
            serviceRequest.ServiceId = Convert.ToInt32(serviceVal);
            serviceRequest.Property_No = propertyNo;
            serviceRequest.Request_Status = 8;
            if (subDeptVal == "2")
            {
                serviceRequest.SubDepartment = "A";
            }
            else
            {
                serviceRequest.SubDepartment = "P";
            }
            serviceRequest.LastDateofPayment = System.DateTime.Now;
            serviceRequest.ReqType = type;
            serviceRequest.Mobile = mobile;
            serviceRequest.Email = email;
            //serviceRequest.Created_By = currentUser.UserEmail;
            ManageCitizenService citizenService = new ManageCitizenService();
            var Result = citizenService.SaveInternalServiceRequest(serviceRequest);
            return Json(Result.ServiceRequestId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOfflineServiceRequest(string rid, string name, string departmentId, string subDepartment, string serviceId, string propertyNo, string address, string mobile, string email, string requestorAddress, string requesterName, string type)
        {
            CitizenServiceRequest serviceRequest = new CitizenServiceRequest();
            var currentUser = (CurrentUserDetail)Session["CurrentUser"];

            serviceRequest.Registration_No = rid;
            serviceRequest.Name = name;
            serviceRequest.ApplicantAddress = address;
            serviceRequest.Property_No = propertyNo;
            serviceRequest.DepartmentId = Convert.ToInt32(departmentId);
            serviceRequest.ServiceId = Convert.ToInt32(serviceId);
            serviceRequest.Request_Status = 8;
            //serviceRequest.Description = description;
            serviceRequest.LastDateofPayment = System.DateTime.Now;
            serviceRequest.ReqType = type;
            serviceRequest.Mobile = mobile;
            serviceRequest.Email = email;
            serviceRequest.RequesterName = requesterName;
            serviceRequest.RequestorAddress = requestorAddress;
            if (subDepartment == "2")
            {
                serviceRequest.SubDepartment = "A";
            }
            else
            {
                serviceRequest.SubDepartment = "P";
            }
            ManageCitizenService citizenService = new ManageCitizenService();
            var Result = citizenService.SaveOfflineServiceRequest(serviceRequest);
            return Json(Result.ServiceRequestId, JsonRequestBehavior.AllowGet);
        }

        //public SaveServiceReqTest()
        //{
        //    CitizenServiceRequest ObjCitizenServiceRequest = new CitizenServiceRequest();

        //    ObjCitizenServiceRequest.Registration_No = "10000004";
        //    ObjCitizenServiceRequest.Property_No = "AA";
        //    ObjCitizenServiceRequest.DepartmentId = 1;
        //    ObjCitizenServiceRequest.ServiceId = 1;
        //    ObjCitizenServiceRequest.Created_By = 1;
        //    ObjCitizenServiceRequest.Request_Status = 1;
        //    ObjCitizenServiceRequest.Description = "Kamdev";
        //    ObjCitizenServiceRequest.AmountTobePaid = 1;
        //    ObjCitizenServiceRequest.LastDateofPayment = System.DateTime.Now;
        //    ObjCitizenServiceRequest.Comment = "Comment";

        //    //var objLst = new List<ServiceRequestDocument>();
        //    //ServiceRequestDocument obj = new ServiceRequestDocument();
        //    //obj.ChkDocumentId = 1;
        //    //obj.DocumentPath = "Document1";
        //    //obj.Uploaded_By = 1;
        //    //obj.Uploaded_Date = System.DateTime.Now;
        //    //objLst.Add(obj);

        //    //ObjCitizenServiceRequest.DocumentList = objLst;
        //    ManageCitizenService objManageCitizenService = new ManageCitizenService();
        //    var Result=objManageCitizenService.SaveServiceRequest(ObjCitizenServiceRequest);

        //    RedirectToAction("CiitizenService", "Customer");
        //}


        public ActionResult GetDocumentList([DataSourceRequest] DataSourceRequest request, int departmentId, int ServiceId)
        {
            ManageCitizenService objManageCitizenService = new ManageCitizenService();
            // var documentLst= objManageCitizenService.GetCheckListDocumentMentsByServiceId_DepartmentId(ServiceId, departmentId);
            DataSourceResult result = null;// documentLst.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files, CitizenServiceRequest model)
        {
            IManageCitizenService objManageCitizenService = new ManageCitizenService();

            if (Session["DocumentLst"] != null)
            {
                var DocumentLst = (List<ServiceRequestDocument>)Session["DocumentLst"];

                var countIndex = 0;
                if (files != null && files.ToList().Count > 0)
                {
                    //foreach (var obj in files)
                    //{
                    //    if (obj != null)
                    //    {
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            byte[] fileData = null;
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                fileData = binaryReader.ReadBytes(Request.Files[countIndex].ContentLength);
                            }

                            string fileName = DocumentLst[countIndex].ChkDocumentId.ToString() + Path.GetExtension(file.FileName);

                            UploadDocument(model.Registration_No, model.ServiceRequestId, fileName, fileData);
                            countIndex++;

                            //var parentDirname = Convert.ToString(model.ServiceRequestId);
                            //var subDirname = Convert.ToString(DocumentLst[countIndex].ChkDocumentId); // to create sub folder

                            //if (file != null && file.ContentLength > 0)
                            //{
                            //    var filePath = Server.MapPath("/uploads");
                            //    filePath += "\\" + parentDirname;

                            //    if (!Directory.Exists(filePath))
                            //        Directory.CreateDirectory(filePath);

                            //    filePath += "\\" + subDirname;
                            //    if (!Directory.Exists(filePath))
                            //        Directory.CreateDirectory(filePath);



                            //    file.SaveAs(
                            //        Path.Combine(filePath + "\\" + DocumentLst[countIndex].ChkDocumentName +
                            //                     Path.GetExtension(file.FileName)));


                        }
                    }

                    if (objManageCitizenService.UpdateRequestStatusByReqId(model.ServiceRequestId))
                    {
                        TempData["completed"] = true;
                        TempData["ServiceRequestId"] = Convert.ToString(model.ServiceRequestId);
                    }
                    else
                    {
                        TempData["completed"] = false;
                    }
                    //    }
                    //}
                }
            }

            //var id = Session["RegistrationID"];
            //var strId = id.ToString();
            //if (string.IsNullOrEmpty(strId))
            //{
            var id = model.Registration_No;
            //}
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
            var obj = _propertyService.GetMobileEmailByReqId(model.ServiceRequestId);
            if (obj != null && propDetails != null)
            {
                propDetails.Mobile = obj.Mobile;
                propDetails.EmailId = obj.Email;
            }
            EmailHelper emailHelper = new EmailHelper();
            if (propDetails != null)
            {
                if (propDetails.EmailId != null && propDetails.EmailId != "")
                {
                    var sbjct = "Dear User,<br><br>We acknowledge the receipt of your application for " + model.ServiceName + " against property registration " + model.Registration_No + ". Your Service Request ID is " + model.ServiceRequestId + ". We will validate the information provided in the application and then proceed with your service request. If any additional information like payment, required documents etc. is/are required, we will communicate you on registered mobile number/email id. You can also track the status of service request on http://www.MyNOIDA.IN. Please keep your service request number for any communication regarding the application.<br>Thank you for using services online.<br><br>Yours sincerely,<br>Noida";
                    emailHelper.Send(propDetails.EmailId, "Acknowledgement Reciept", sbjct);
                }
                if (!string.IsNullOrEmpty(propDetails.Mobile))
                    //SMSSend(propDetails.Mobile, "Dear Applicant, Your service request number " + model.ServiceRequestId + " has been submitted sucessfully.");
                    SMSSend(propDetails.Mobile, "Dear User, Your service request number " + model.ServiceRequestId + ". Please check the details online at mynoida.in. Regards, http://mynoida.in.");
            }
            if (Convert.ToInt32(Session["RoleId"]) == 2)
            {
                return RedirectToAction("ServiceRequest", "Customer", new { id = CommonHelper.Encode(model.Registration_No) });
            }
            else
            {
                TempData["MesgAdd"] = true;
                return RedirectToAction("Dashboard", "Home");
            }
            //return RedirectToAction("CitizenService", new { id = propertyDetail.RegistrationNo, optString = string.Empty });
        }

        private bool FTPDirectoryExists(string FtpPath, string DirectoryToCreate, string FTPUserName, string FTPUserPassword)
        {
            bool directoryExists;
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpPath + "/" + DirectoryToCreate));
            ftpClient.Credentials = new System.Net.NetworkCredential(FTPUserName.Normalize(), FTPUserPassword.Normalize());
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;

            try
            {
                using (ftpClient.GetResponse())
                {
                    directoryExists = true;
                }
            }
            catch (WebException)
            {
                directoryExists = false;
            }

            return directoryExists;
        }
        private bool FTPDirectoryCreate(string FtpPath, string DirectoryToCreate, string FTPUserName, string FTPUserPassword)
        {
            bool IsDirectoryCreated = false;
            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpPath + "/" + DirectoryToCreate));
            ftpClient.Credentials = new System.Net.NetworkCredential(FTPUserName.Normalize(), FTPUserPassword.Normalize());
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                using (ftpClient.GetResponse())
                {
                    IsDirectoryCreated = true;
                }
            }
            catch (WebException)
            {
                IsDirectoryCreated = false;
            }

            return IsDirectoryCreated;
        }
        private void UploadDocument(string RegistrationId, int RequestId, string filename, byte[] fileData)
        {
            string CompleteDPath = "ftp://52.172.186.197/UploadDocuments/";


            string userName = "Kamdev";
            //WebRequest reqObj = WebRequest.Create(CompleteDPath + FileName);
            string username = ConfigurationManager.AppSettings["FTPUsername"];
            string pass = ConfigurationManager.AppSettings["FTPPassword"];

            #region Directory check & create

            bool IsCreatedDirectoryRequestId = false;
            bool IsCreatedDocumentIdFolder = false;


            bool IsRequestIdDirectory = FTPDirectoryExists(CompleteDPath, RegistrationId, username, pass);
            if (IsRequestIdDirectory == false)
            {
                IsCreatedDirectoryRequestId = FTPDirectoryCreate(CompleteDPath, RegistrationId, username, pass);

            }
            else
            {
                IsCreatedDirectoryRequestId = true;
            }
            if (IsCreatedDirectoryRequestId == true)
            {
                bool IsDocumentIdDirectory = FTPDirectoryExists(CompleteDPath, RegistrationId + "/" + RequestId, username, pass);
                if (IsDocumentIdDirectory == false)
                {
                    IsCreatedDocumentIdFolder = FTPDirectoryCreate(CompleteDPath, RegistrationId + "/" + RequestId, username, pass);

                }
                else
                {
                    IsCreatedDocumentIdFolder = true;
                }
            }
            #endregion

            if (IsCreatedDirectoryRequestId == true && IsCreatedDocumentIdFolder == true)
            {
                FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(CompleteDPath + "/" + Convert.ToString(RegistrationId) + "/" + Convert.ToString(RequestId) + "/" + filename));
                ftpClient.Credentials = new System.Net.NetworkCredential(username.Normalize(), pass.Normalize());
                ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;


                ftpClient.ContentLength = fileData.Length;
                byte[] buffer = new byte[4097];
                int bytes = fileData.Length;
                int total_bytes = (int)fileData.Length;

                try
                {
                    System.IO.Stream rs = ftpClient.GetRequestStream();
                    //System.IO.FileStream fs = fileData.op

                    while (total_bytes > 0)
                    {
                        rs.Write(fileData, 0, bytes);
                        total_bytes = total_bytes - bytes;
                    }
                    rs.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                var value = uploadResponse.StatusDescription;
                uploadResponse.Close();


                // string server = System.Web.HttpContext.Current.Server.MapPath("~") + @"\uploads\" + ServiceeqId + @"\" + CheckdocId + @"\";

                //var path = string.Format(@"{0}", server);
                //if (Directory.Exists(path))
                //{
                //    List<String> Files = Directory
                //            .GetFiles(server, "*.*", SearchOption.AllDirectories).ToList();

                //    foreach (var file in Files)
                //    {
                //        FileInfo mFile = new FileInfo(file);
                //        ftpClient.ContentLength = mFile.Length;
                //        byte[] buffer = new byte[4097];
                //        int bytes = 0;
                //        int total_bytes = (int)mFile.Length;
                //        System.IO.FileStream fs = mFile.OpenRead();
                //        try
                //        {
                //            System.IO.Stream rs = ftpClient.GetRequestStream();
                //            while (total_bytes > 0)
                //            {
                //                bytes = fs.Read(buffer, 0, buffer.Length);
                //                rs.Write(buffer, 0, bytes);
                //                total_bytes = total_bytes - bytes;
                //            }
                //            fs.Close();
                //            rs.Close();
                //        }
                //        catch (Exception e)
                //        {
                //            throw e;
                //        }

                //        //fs.Flush();

                //        FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                //        var value = uploadResponse.StatusDescription;
                //        uploadResponse.Close();
                //    }
                //}
            }


        }
        private void BindHtmlForUpload(int ServiceRequestId)
        {

            ManageCitizenService objManageCitizenService = new ManageCitizenService();
            var DocumentLst = objManageCitizenService.GetCheckListDocumentMentsByServiceId_DepartmentId(ServiceRequestId);
            string divMain = "";
            int docCounter = 1;
            foreach (var item in DocumentLst)
            {


                //divMain = divMain + "<div class='gServiceTableRow'><div class='gServiceTableCell'><span>" + item.ChkDocumentId
                divMain = divMain + "<div class='gServiceTableRow'><div class='gServiceTableCell'><span>" + docCounter
                                       + "</span></div><div class='gServiceTableCell'><span>" + item.ChkDocumentName
                                       + "</span></div><div class='gServiceTableCell'><input type='file' class='single' name='files' /></div></div>";

                docCounter++;

                //divMain = divMain + "<div class='row'><div class='col-sm-2'>"
                //       + "<span id='" + item.ChkDocumentId + "'>" + item.ChkDocumentId
                //       + "</span></div><div class='col-sm-4'><span id='" + item.ChkDocumentName + "'>" + item.ChkDocumentName
                //       + "</span></div><div class='col-sm-6'><span><input type='file' class='single' name='files' /></span></div>";


            }
            ViewBag.uploadhtml = divMain;


            //put in session
            Session["DocumentLst"] = DocumentLst;

            //    <div class="service-grid">
            //// save on to PMS
            //var serviceRequest = new ServiceRequest();
            //var pmsClient = new PmsApirequestHelper();

            //serviceRequest.DepartmentId = Convert.ToInt32(deptVal);
            //serviceRequest.ServiceId = Convert.ToInt32(serviceVal);
            //serviceRequest.Registration_No = regNo;
            //serviceRequest.Description = description;
            //pmsClient.PostRequestJson(System.Configuration.ConfigurationManager.AppSettings["PmsApiUrl"] + "SaveServiceRequest/", JsonConvert.SerializeObject(serviceRequest));

        }

        public ActionResult SaveCitizenServiceRequest(NoidaServiceRequestModel req)
        {
            //_propertyService = new PropertyService();
            //var rslt = _propertyService.SaveServiceReq(Convert.ToInt32(deptVal), Convert.ToInt32(serviceVal), description, regNo);


            CitizenServiceRequest serv = new CitizenServiceRequest();
            serv.Registration_No = req.RegistrationNo;
            serv.DepartmentId = Convert.ToInt32(req.DepartmentId);
            serv.ServiceId = Convert.ToInt32(req.ServiceId);
            serv.Description = req.Description;
            serv.Property_No = null;
            serv.Comment = null;
            serv.DocumentList = null;

            ServiceRequestDocument document = new ServiceRequestDocument();
            document.ChkDocumentId = 0;
            document.ChkDocumentName = "doc";
            document.DocumentPath = "path";
            document.Uploaded_Date = DateTime.Now;

            IManageCitizenService service = new ManageCitizenService();
            var flag = service.SaveServiceRequest(serv);

            //return RedirectToAction("CitizenService", new { id = flag.Registration_No, optString="" });
            // return View("CitizenService");
            //  return RedirectToAction("CitizenService", new { id = 1 });

            return RedirectToAction("CitizenService", "Customer", new { id = flag.ServiceRequestId });

        }
        //public ActionResult CitizenService(int? id)
        //{
        //    ManageCitizenService serv = new ManageCitizenService();

        //    var propDetails = serv.GetServiceRequestDetails((int)id);
        //    //var propDetails = new CitizenServiceRequest();
        //    _propertyService = new PropertyService();
        //    return View("CitizenService", propDetails);
        //}


        public ActionResult ServiceRequests()
        {
            return View();
        }

        public ActionResult FileUpload(int? id)
        {
            ManageCitizenService serv = new ManageCitizenService();

            var propDetails = serv.GetServiceRequestDetails((int)id);

            _propertyService = new PropertyService();
            BindHtmlForUpload((int)id);
            //if (Convert.ToInt32(Session["RoleId"]) == 2)
            //{
            //    return View("CitizenService", propDetails);
            //}
            //else
            //    return View("CitizenServiceEmployee", propDetails);
            return View(propDetails);
        }

        public ActionResult SR_Read([DataSourceRequest] DataSourceRequest request)
        {
            _propertyService = new PropertyService();
            var rsltLst = _propertyService.SR_Read(request);
            return Json(rsltLst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Citizen()
        {
            var id = Session["RegistrationID"];
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
            {
                PropertyFilterObj.RegistrationId = _RegistrationId;
                DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
                PropertyDetail propertyDetail = SetPropertyDetailObject(propDetails);
                //var role = Session["RoleName"];
                //ViewBag.role = role;
                // ViewBag.Remarks =  _propertyService.GetPropertyRemark(Convert.ToString(id));
                return View(propertyDetail);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            //return View();
        }

        public ActionResult CitizenPropertyDetail(string id, string optString)
        {
            int proid = !string.IsNullOrEmpty(id) ? Convert.ToInt32(CommonHelper.Decode(id)) : 0;
            var propertyDetail = GetCitizenPropertyDetail(proid, optString);
            return View(propertyDetail);
        }
        public ActionResult PaymentDetail(int id, string optString)
        {
            var propertyDetail = GetCitizenPropertyDetail(id, optString);
            return View(propertyDetail);
        }
        public ActionResult ServiceRequest(string id, string optString)
        {
            int proid = !string.IsNullOrEmpty(id) ? Convert.ToInt32(CommonHelper.Decode(id)) : 0;
            //int proid = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;
            var propertyDetail = GetCitizenPropertyDetail(proid, optString);
            return View(propertyDetail);
        }
        public ActionResult JalPaymentDetail(int id, string optString)
        {
            var propertyDetail = GetCitizenPropertyDetail(id, optString);
            return View(propertyDetail);
        }
        public ActionResult LitigationHistory(int id, string optString)
        {
            var propertyDetail = GetCitizenPropertyDetail(id, optString);
            return View(propertyDetail);
        }
        public ActionResult TransferDetail(int id, string optString)
        {
            var propertyDetail = GetCitizenPropertyDetail(id, optString);
            return View(propertyDetail);
        }
        public ActionResult Documents(int id, string optString)
        {
            var propertyDetail = GetCitizenPropertyDetail(id, optString);
            return View(propertyDetail);
        }
        public ActionResult CitizenRemarks(int id, string optString)
        {
            var propertyDetail = GetCitizenPropertyDetail(id, optString);
            return View(propertyDetail);
        }
        public ActionResult CitizenService(int? id)
        {
            //bindHtmlForUpload();
            ManageCitizenService serv = new ManageCitizenService();

            var propDetails = serv.GetServiceRequestDetails((int)id);

            _propertyService = new PropertyService();
            BindHtmlForUpload((int)id);
            if (Convert.ToInt32(Session["RoleId"]) == 2)
            {
                return View("CitizenService", propDetails);
            }
            else
                return View("CitizenServiceEmployee", propDetails);

        }

        public JsonResult GetRIDsByDeptt(int deptt)
        {
            ManageCitizenService serv = new ManageCitizenService();
            var lst = serv.GetRIDsByDeptt(deptt);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetServiceRequestDetails()
        {
            return View("GetServiceRequestDetails", null);
        }

        public NoidaServiceRequestModel SetNoidaServiceRequestModel(DtoPropertyDetails propDetails)
        {
            NoidaServiceRequestModel propertyDetail = new NoidaServiceRequestModel();

            propertyDetail.PropertyId = propDetails.PropertyId;
            propertyDetail.Name = propDetails.Name;
            propertyDetail.RegistrationNo = propDetails.RegistrationNo;
            propertyDetail.PropertyType = propDetails.PropertyType;
            propertyDetail.Sector = propDetails.Sector;
            propertyDetail.Status = propDetails.Status;
            propertyDetail.Block = propDetails.Block;
            //propertyDetail.Scheme = propDetails.Scheme;
            propertyDetail.Area = propDetails.Area;

            propertyDetail.CustomerAddress = propDetails.CustomerAddress;
            propertyDetail.EmailId = propDetails.EmailId;
            propertyDetail.PhoneNo = propDetails.PhoneNo;
            propertyDetail.Mobile = propDetails.Mobile;
            propertyDetail.PlotNumber = propDetails.PlotNumber;
            propertyDetail.PropertyNumber = propDetails.PropertyNumber;
            propertyDetail.PropertyCategory = propDetails.PropertyCategory;

            return propertyDetail;
        }

        public ActionResult GetAllChecklistServiceData(int serviceRequestId)
        {
            //CitizenServiceRequest serv = new CitizenServiceRequest();
            ////serv.Registration_No = regNo;
            //serv.DepartmentId = Convert.ToInt32(deptVal);
            //serv.ServiceId = Convert.ToInt32(serviceVal);
            //serv.Description = description;

            IManageCitizenService service = new ManageCitizenService();
            var flag = service.GetCheckListDocumentMentsByServiceId_DepartmentId(serviceRequestId);

            //var flag = service.SaveServiceRequest(serv);

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult OnlineHelp()
        {
            return View();
        }


        #region CitizenRequest

        public ActionResult ManageCitizenRequests()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetCitizenRequests([DataSourceRequest] DataSourceRequest req)
        {
            _propertyService = new PropertyService();
            var data = _propertyService.GetCitizenRequests(req);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCustomerServiceRequest([DataSourceRequest]DataSourceRequest req)
        {

            var data = new DataSourceResult();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewRequestDetails(int id)
        {
            _propertyService = new PropertyService();
            var details = _propertyService.GetRequestDetailsById(id);
            return View(details);
        }

        //public JsonResult SaveRequestDetails(int id, decimal? serviceFee, decimal? duesAmnt, string comment, int temp)
        //{
        //    _propertyService = new PropertyService();
        //    var flag = _propertyService.SaveRequestDetails(id, serviceFee, duesAmnt, comment, temp);
        //    return Json(flag, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetDocumentDetails([DataSourceRequest] DataSourceRequest req, int rid = 0, int id = 0)
        {
            FtpHandler ObjFtpHandler = new FtpHandler();
            //_propertyService = new PropertyService();
            //var lst = _propertyService.GetDocumentDetails(req, rid, id);
            //string path = System.Configuration.ConfigurationManager.AppSettings["RootPath"] + "\\" + rid + "\\ServiceRequest" + "\\" + id;
            //List<string> allFilesOld = DirSearch(path);
            //foreach (string file in allFilesOld)
            //{
            //    string str = file;
            //    //if (str.Contains(" "))
            //    //{
            //    //    str = str.Replace(" ", "");
            //    //    RenameFiles(file, str, path);
            //    //}
            //}
            //List<string> allFiles = DirSearch(path);
            //List<PropertyDocument> lstDocumentDetail = new List<PropertyDocument>();
            //foreach (string file in allFiles)
            //{
            //    foreach (var item in lst)
            //    {
            //        string str = file;
            //        if (file.ToLower() == item.DocumentName.ToLower())
            //        {
            //            string str1 = string.Empty;
            //            //str1 = str.Substring(0, str.Length - 4);
            //            //str1 = str1.Substring(9, str1.Length - 9);
            //            PropertyDocument objDocumentDetail = new PropertyDocument();

            //            objDocumentDetail.DocumentPath = "ftp:\\" + System.Configuration.ConfigurationManager.AppSettings["FTPUsername"] + ":" + System.Configuration.ConfigurationManager.AppSettings["FTPPassword"] + "@" + System.Configuration.ConfigurationManager.AppSettings["SiteRootPath"] + "//" + rid + "//" + file;
            //            objDocumentDetail.DocumentName = item.DocumentType;
            //            objDocumentDetail.RegistrationId = rid;
            //            lstDocumentDetail.Add(objDocumentDetail);
            //        }
            //    }
            //}
            //return Json(lstDocumentDetail, JsonRequestBehavior.AllowGet);
            //_propertyService = new PropertyService();
            //var lst = _propertyService.GetDocumentDetails(req, rid, id);
            string path = ObjFtpHandler.GetDocumentPath(rid, id, string.Empty, true);//System.Configuration.ConfigurationManager.AppSettings["FTPRootPath"] + "//" + rid + "//" + id;
            //List<string> allFilesOld = DirSearch(path);
            //foreach (string file in allFilesOld)
            //{
            //    string str = file;
            //    //if (str.Contains(" "))
            //    //{
            //    //    str = str.Replace(" ", "");
            //    //    RenameFiles(file, str, path);
            //    //}
            //}
            List<string> allFiles = ObjFtpHandler.DirSearch(path);
            List<PropertyDocument> lstDocumentDetail = new List<PropertyDocument>();
            foreach (string file in allFiles)
            {
                //foreach (var item in lst)
                //{
                string str = file;
                //if (file.ToLower() == item.DocumentName.ToLower())
                //{
                string str1 = string.Empty;
                //str1 = str.Substring(0, str.Length - 4);
                //str1 = str1.Substring(9, str1.Length - 9);
                PropertyDocument objDocumentDetail = new PropertyDocument();

                //objDocumentDetail.DocumentPath = "ftp:\\" + System.Configuration.ConfigurationManager.AppSettings["FTPUsername"] + ":" + System.Configuration.ConfigurationManager.AppSettings["FTPPassword"] + "@" + System.Configuration.ConfigurationManager.AppSettings["SiteRootPath"] + "//" + rid + "//" + file;
                objDocumentDetail.DocumentPath = ObjFtpHandler.GetDocumentPath(rid, id, file, false);//"ftp:\\" + System.Configuration.ConfigurationManager.AppSettings["FTPUsernameview"] + ":" + System.Configuration.ConfigurationManager.AppSettings["FTPPasswordview"] + "@" + System.Configuration.ConfigurationManager.AppSettings["FTPSiteRootPath"] + "//" + rid + "//" + id + "//" + file;
                objDocumentDetail.DocumentName = file;
                objDocumentDetail.RegistrationId = rid;
                lstDocumentDetail.Add(objDocumentDetail);
                //}
                //}
            }
            var lstdocument = lstDocumentDetail.ToDataSourceResult(req);
            return Json(lstdocument, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// Used for searching Files in FTP Server.
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //private List<string> DirSearch(string path)
        //{
        //    List<String> files = new List<String>();
        //    try
        //    {
        //        var request = CreateRequest(path, WebRequestMethods.Ftp.ListDirectory);
        //        using (var response = (FtpWebResponse)request.GetResponse())
        //        {
        //            using (var stream = response.GetResponseStream())
        //            {
        //                using (var reader = new StreamReader(stream, true))
        //                {
        //                    while (!reader.EndOfStream)
        //                    {
        //                        files.Add(reader.ReadLine());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception excpt)
        //    {
        //    }

        //    return files;
        //}

        //private FtpWebRequest CreateRequest(string uri, string method)
        //{
        //    var r = (FtpWebRequest)WebRequest.Create(uri);

        //    r.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["FTPUsername"], System.Configuration.ConfigurationManager.AppSettings["FTPPassword"]);
        //    r.Method = method;
        //    return r;
        //}

        #endregion

        public ActionResult ViewRequestDetail(int id)
        {
            _propertyService = new PropertyService();
            _manageCitizenService = new ManageCitizenService();
            ViewBag.gridDetails = _manageCitizenService.GetDetailsByRequestId(id);
            var details = _propertyService.GetRequestDetailsById(id);
            return View(details);
        }

        public JsonResult GetMortgageType()
        {
            _propertyService = new PropertyService();
            var lstMortgageType = _propertyService.GetMortgageType();
            return Json(lstMortgageType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPreviousLoanNoc()
        {
            _propertyService = new PropertyService();
            var lstPreviousLoanNoc = _propertyService.GetMortgagePrevLoan();
            return Json(lstPreviousLoanNoc, JsonRequestBehavior.AllowGet);
        }

        //registrationno is Service Request No.
        public JsonResult SaveMortgageReq(int rid, int registrationno, string BankName, string BranchAddress, decimal SanctionedAmount, string MortgageType, short PreviousLoanNoc, string desc)
        {
            _propertyService = new PropertyService();
            var lstPreviousLoanNoc = _propertyService.SaveMortgageReq(rid, registrationno, BankName, BranchAddress, SanctionedAmount, MortgageType, PreviousLoanNoc, desc);
            return Json(lstPreviousLoanNoc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTransferRequest(int transType, int transSubType, string firstName, string middleName, string lastName, string relativeName, string motherName, string mobileNo, string email, string correspondenceAdd, string permanentAdd, string PAN, int? occupation, int rId, int serviceId, string gender, string cmpnyName, string signAuth, string RO, int reqID, string desc)
        {
            _propertyService = new PropertyService();
            var flag = _propertyService.SaveTransferRequest(transType, transSubType, firstName, middleName, lastName, relativeName, motherName, mobileNo, email, correspondenceAdd, permanentAdd, PAN, occupation, rId, serviceId, gender, cmpnyName, signAuth, RO, reqID, desc);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveGpaRequest(GPAModel ObjGpaModel)
        {
            _propertyService = new PropertyService();
            var flag = _propertyService.SavegpaRequest(ObjGpaModel);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOccupation()
        {
            _propertyService = new PropertyService();
            var lst = _propertyService.GetOccupation();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGender()
        {
            _propertyService = new PropertyService();
            var lst = _propertyService.GetGender();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferTypes()
        {
            _propertyService = new PropertyService();
            var lst = _propertyService.GetTransferTypes();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferSubTypes(int TransferType)
        {
            _propertyService = new PropertyService();
            var lst = _propertyService.GetTransferSubTypes(TransferType);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // To Bind Change Type in drop down.
        public ActionResult GetChangeType()
        {
            _propertyService = new PropertyService();
            var lstChangeType = _propertyService.BindDDL(CHANGEDDLTYPE);
            return Json(lstChangeType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetType()
        {
            _propertyService = new PropertyService();
            var lstChangeType = _propertyService.BindDDL(TYPE);
            return Json(lstChangeType, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDirectorDetails(DataSourceRequest req, int rid)
        {
            _propertyService = new PropertyService();
            var lstDirectorDetails = _propertyService.GetDirectorDetails(req, rid);
            return Json(lstDirectorDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFirmStatus()
        {
            _propertyService = new PropertyService();
            var lstFirmStatus = _propertyService.BindDDL(FIRMSTATUS);
            return Json(lstFirmStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMutationRequest(DateTime transDeedDate, string bahiNo, string bahiZildNo, string bahiPgNo, string SINo, int rId, int serviceId, string desc)
        {
            _propertyService = new PropertyService();
            var flag = _propertyService.SaveMutationRequest(transDeedDate, bahiNo, bahiZildNo, bahiPgNo, SINo, rId, serviceId, desc);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[AllowAnonymous]
        public JsonResult SaveRentServiceReq(int rid, int serviceId, string tenantName, string tenantProject, decimal rentingCharge, DateTime rentingDate, int rentDuration, string desc)
        {
            _propertyService = new PropertyService();
            var lstPreviousLoanNoc = _propertyService.SaveRentServiceReq(rid, serviceId, tenantName, tenantProject, rentingCharge, rentingDate, rentDuration, desc);
            return Json(lstPreviousLoanNoc, JsonRequestBehavior.AllowGet);
        }

        // To Save CIC for Director        
        public ActionResult SaveCIC(int rid, int registrationno, decimal directorShare, int changeType, string directorName, int type, string desc, decimal cicCharge = 0)
        {
            var flag = false;
            _propertyService = new PropertyService();
            flag = _propertyService.SaveCIC(rid, registrationno, directorShare, changeType, cicCharge, directorName, desc, type);
            return Json(flag);
        }

        public ActionResult SaveCICFirmName(int rid, int registrationno, int newFirmStatus, string newFirmName, string desc, decimal ciccharge = 0)
        {
            var flag = 0;
            _propertyService = new PropertyService();
            flag = _propertyService.SaveCICFirmName(rid, registrationno, newFirmStatus, newFirmName, desc, ciccharge);
            return Json(flag);
        }

        public ActionResult SaveForFirmProduct(int rid, int registrationno, string newFirmProduct, string desc, decimal ciccharge = 0)
        {
            var flag = 0;
            _propertyService = new PropertyService();
            flag = _propertyService.SaveForFirmProduct(rid, registrationno, newFirmProduct, desc, ciccharge);
            return Json(flag);
        }

        public JsonResult RemoveRecord(int dirID)
        {
            _propertyService = new PropertyService();
            var flag = _propertyService.RemoveRecord(dirID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadReceipt(int challanId)
        {
            var flag = true;
            _propertyService = new PropertyService();
            var challanFile = _propertyService.DownloadChallan(challanId);

            return new FilePathResult("challanFile.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            //return Json(flag,JsonRequestBehavior.AllowGet);
        }


        //public ActionResult SendMessage()
        //{
        //    var flag = true;
        //var id = Session["RegistrationID"];
        //_propertyService = new PropertyService();
        //PropertyFilterObj = new DtoPropertyFilter();
        //int _RegistrationId = 0;
        //if (int.TryParse(Convert.ToString(id), out _RegistrationId))
        //    PropertyFilterObj.RegistrationId = _RegistrationId;
        //DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
        //EmailHelper emailHelper = new EmailHelper();
        //if (propDetails.EmailId != null && propDetails.EmailId != "")
        //    emailHelper.Send(propDetails.EmailId, "Email Subject", "Dear Applicant,<br><br>");
        //SMSSend(propDetails.Mobile, "Message Text");
        //    return Json(flag,JsonRequestBehavior.AllowGet);
        //}

        private void SMSSend(string mobNo, string p)
        {
            WebClient client = new WebClient();
            string baseurl = ConfigurationManager.AppSettings["SMSsend"].ToString() + ConfigurationManager.AppSettings["SMSUsername"].ToString() + "&password=" + ConfigurationManager.AppSettings["SMSPassword"].ToString() + "&sendername=" + ConfigurationManager.AppSettings["SMSSenderID"].ToString() + "&mobileno=" + mobNo + "&message=" + p;
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
        }

        /// <summary>
        /// Returns DDL with "Yes" and "No" options
        /// </summary>
        /// <returns></returns>
        public JsonResult YesNoDDL()
        {
            _commonService = new CommonService();
            var lst = _commonService.YesNoDDL();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveExtensionRequest(int rid, int requestId, DateTime extensionDueDate, DateTime extensionGivenDate, string desc)
        {
            var flag = 0;
            _propertyService = new PropertyService();
            flag = _propertyService.SaveExtensionRequest(rid, requestId, extensionDueDate, extensionGivenDate, desc);
            return Json(flag);
        }

        public JsonResult GetPropertyDetailsForExtension(DataSourceRequest request, int departmentId, int ServiceId)
        {
            DataSourceResult result = _propertyService.GetPropertyDetailsForExtension(request, departmentId, ServiceId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult AddCitizenRequest()
        {
            //var id = Session["RegistrationID"];
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            //if (int.TryParse(Convert.ToString(id), out _RegistrationId))
            //    PropertyFilterObj.RegistrationId = _RegistrationId;
            //DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 2);
            PropertyDetail propertyDetail = new PropertyDetail();
            //p
            //var role = Session["RoleName"];
            //ViewBag.role = role;
            // ViewBag.Remarks =  _propertyService.GetPropertyRemark(Convert.ToString(id));
            return View(propertyDetail);
        }

        public JsonResult GetDepttByRID(int RID)
        {
            _commonService = new CommonService();
            var details = _commonService.GetDepttByRID(RID);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadAcknowledgeReceipt()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(ConfigurationManager.AppSettings["AcknowledgementTemplate"]));
            //byte[] fileBytes = System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings["AcknowledgementTemplate"]);
            string fileName = "Acknowledgement.docx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public JsonResult SaveOtherRequest(string desc, int reqID)
        {
            _propertyService = new PropertyService();
            var flag = _propertyService.SaveOtherRequest(desc, reqID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceReport()
        {
            return View();
        }
        public JsonResult GetServiceRequestReport([DataSourceRequest] DataSourceRequest request)
        {
            _propertyService = new PropertyService();
            DataSourceResult data = _propertyService.GetServiceRequestReport(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportingService([DataSourceRequest] DataSourceRequest request)
        {
            _propertyService = new PropertyService();
            DataSourceResult data = _propertyService.GetReportingService(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartmentList()
        {
            ICommonService commonService = new CommonService();
            //List<DDList> departmentList = commonService.GetDepartmentList();
            var departmentList = commonService.GetDepartmentList();
            return Json(departmentList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        public ActionResult RaiseServiceRequest()
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            PropertyDetail propertyDetail = new PropertyDetail();
            return View(propertyDetail);
        }

        public ActionResult GetServicesList(int? departmentId)
        {
            ICommonService commonService = new CommonService();
            //List<DDList> departmentList = commonService.GetServicesList(departmentId);
            var departmentList = commonService.GetServicesList(departmentId);
            return Json(departmentList, JsonRequestBehavior.AllowGet);
        }
    }
}