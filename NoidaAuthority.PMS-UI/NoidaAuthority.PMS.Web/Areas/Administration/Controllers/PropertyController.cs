using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NoidaAuthority.PMS.Service.ManageUsers;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Entity.NaUser;
using System.IO;

namespace NoidaAuthority.PMS.Web.Areas.Administration.Controllers
{
    public class PropertyController : Controller //WebBaseController
    {
        IPropertyService _propertyService;
        DtoPropertyFilter PropertyFilterObj;
        ICommonService _commonService;
        IRequestService _requestService;
        ICustomerService _customerService;
        private ILogService logService = null;
        private IManageUserService manageUserService;       

        public PropertyController(IRequestService requestService, ICommonService commonService, ICustomerService customerService, IPropertyService propertyService)
        {
            _propertyService = propertyService;
            _requestService = requestService;
            _commonService = commonService;
            _customerService = customerService;
            manageUserService = new ManageUserService();
            logService = new LogService();
        }
        
        // GET: /Property/
        public ActionResult Index()
        {
            ViewBag.FilterString = CreateFilterDisplayString("Property Listing");
            return View();
        }

        public ActionResult Dashboard()
        {
            var pCount = _commonService.GetPropertyCount();
            ViewData["PropCount"] = pCount;
            return View();
        }

        public ActionResult Revenue()
        {
            return View();
        }
        public ActionResult InProgress()
        {
            return View();
        }
        public ActionResult MainLocation()
        {

            return View();
        }
        public ActionResult Location()
        {

            return View();
        }
        public ActionResult AdvanceSearch()
        {
            return View();
        }

        public ActionResult PropertySearch()
        {
            //ViewBag.FilterString = CreateFilterDisplayString("Property Listing");
            return View();
        }
        // Get Properties by Payments Status
        public ActionResult Payments()
        {
            ViewBag.FilterString = CreateFilterDisplayString("Payment Report");
            return View();
        }

        // Get Legal Properties 
        public ActionResult Litigation()
        {
            ViewBag.FilterString = CreateFilterDisplayString("Legal Report");
            return View();
        }
        // Get Complaint for Properties 
        public ActionResult Complaints()
        {
            ViewBag.FilterString = CreateFilterDisplayString("Complaints");
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult ServiceRequest(int? id)
        {
            ServiceRequestViewModel model = new ServiceRequestViewModel();
            if (id != null && id > 0)
            {
                model = _requestService.GetServiceRequestDetailById(id);
                return View(model);
            }
            else return View(model);
        }

        public ActionResult ServiceDetail(int? id)
        {
            if (id != null && id > 0)
            {
                ServiceRequestViewModel service = _requestService.GetServiceRequestDetailById(id);
                return View(service);
            }
            else return RedirectToAction("Index");
        }

        public ActionResult ServiceReport()
        {
            return View();
        }

        // GET: /Property/
        public ActionResult PropertyDetail(int? id)
        {
            //_propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);
            PropertyDetail propertyDetail = SetPropertyDetailObject(propDetails);
            return View(propertyDetail);
            //if (id != null)
            //{
            //    var detail = _customerService.GetCustomerDetails(Convert.ToInt32(id));
            //    return View(detail);
            //}
            //else return RedirectToAction("Dashboard", "Admin", new { area="Administrator"});
        }

        //added by Shatrughna
        // GET: /Property/
        [AllowAnonymous]
        public JsonResult PropertyDetailJson(int id)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;

            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);

            return Json(propDetails);
        }

        //added by Shatrughna kumar
        [AllowAnonymous]
        public JsonResult CheckRegistationIDForNACustomer(String RegistrationId)
        {
            var flag = false;
            try
            {
                var validateRegistrationId = new CustomMemberShip();
                flag = validateRegistrationId.CheckRegistationIDforNAcustomer(RegistrationId);
            }
            catch (Exception ex)
            {
                logService.LogError("Error in ChecRegistrationIdForNAcustomerRegistation", ex);
                throw;
            }
            return Json(flag);
        }

        // GET: /Property/
        [HttpGet]
        public JsonResult PropertyDetailJson(string id)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            PropertyFilterObj = CreatePropertyFilter();
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);
            PropertyDetail propertyDetail = new PropertyDetail();
            if (propDetails != null)
            {
                propertyDetail = SetPropertyDetailObject(propDetails);
            }
            return Json(propertyDetail, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetProperties([DataSourceRequest] DataSourceRequest req, List<string> options)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilter();
            if (Session["DeptId"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["pt"]))
                {
                    PropertyFilterObj.PropertyTypeId = Request.QueryString["pt"];
                }
                else if (Session["DeptId"].ToString() == "0")
                    PropertyFilterObj.PropertyTypeId = null;
                else
                    PropertyFilterObj.PropertyTypeId = Session["DeptId"].ToString();
            }
            var propertyList = _propertyService.GetPropertyList(PropertyFilterObj, req);
            var wrapped = new { data = propertyList.Data, total = propertyList.Total };
            //foreach (DtoProperty objDtoProperty in propertyList)
            //{
            //    objDtoProperty.Block = objDtoProperty.Block.Trim();
            //}
            //Following added to resolve the exception "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property."
            return Json(wrapped, JsonRequestBehavior.AllowGet);
            //return new JsonResult()
            //{
            //    Data = propertyList,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            //    MaxJsonLength = Int32.MaxValue
            //};
        }

        public JsonResult GetPropertiesSearch([DataSourceRequest] DataSourceRequest req, List<string> options, DtoPropertyFilter PropertyFilterObj)
        {
            _propertyService = new PropertyService();
            var propertyList = _propertyService.GetPropertySearchList(PropertyFilterObj, req);
            var wrapped = new { data = propertyList.Data, total = propertyList.Total };
            //Following added to resolve the exception "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property."
            return Json(propertyList, JsonRequestBehavior.AllowGet);
        }

        // Get Properties by Payment Status
        public JsonResult GetPropertiesByPaymentStatus()
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilterForFinance();
            if (PropertyFilterObj.SearchType == "Defaulter")
            {
                PropertyFilterObj.PaymentStatus = 2;
            }
            IEnumerable<DtoDepartmentPaymentDetails> propertyListByPayments = _propertyService.GetPropertiesByPaymentStatus(PropertyFilterObj);
            return Json(propertyListByPayments, JsonRequestBehavior.AllowGet);
        }

        // Get Properties by Legal Doc
        public JsonResult GetLegalProperties()
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilter();
            PropertyFilterObj.SearchType = Contants.SearchType.Legal;
            IEnumerable<DtoProperty> propertyListByPayments = _propertyService.GetPropertyList(PropertyFilterObj);
            return Json(propertyListByPayments, JsonRequestBehavior.AllowGet);
        }
        // Get Properties by Legal Doc
        public JsonResult GetLegalPropertiesByCourt(int id)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilter();
            PropertyFilterObj.LegalTypeID = id;
            IEnumerable<DtoLegalwithFarmerDetails> propertyListByPayments = _propertyService.GetLegalPropertiesofHighCourt(PropertyFilterObj);
            return Json(propertyListByPayments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPropertyPaymentHistory(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoPropertyPaymentHistory> propertyPaymentHistory = _propertyService.GetPropertyPaymentHistory(PropertyFilterObj);

            return Json(propertyPaymentHistory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPaymentList()
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilter();
            IEnumerable<DtoPropertyPaymentHistory> propertyPaymentHistory = _propertyService.GetPaymentList(PropertyFilterObj);
            //return Json(propertyPaymentHistory, JsonRequestBehavior.AllowGet);
            //Following added to resolve the exception "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property."
            return new JsonResult()
            {
                Data = propertyPaymentHistory,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public JsonResult GetPropertyComplaintList()
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = CreatePropertyFilter();
            IEnumerable<DtoPropertyComplaintList> propertyPaymentHistory = _propertyService.GetPropertyComplaintList(PropertyFilterObj);
            return Json(propertyPaymentHistory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPropertyComplainHistory(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoPropertyComplaintList> propertyComplaintDetails = _propertyService.GetPropertyComplaintListByRegistrationId(PropertyFilterObj);
            return Json(propertyComplaintDetails, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetTransferHistory(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoTransferHistory> propertyTransferHistory = _propertyService.GetTransferHistory(PropertyFilterObj);
            return Json(propertyTransferHistory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLegalHistory(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DtoLegalHistory> propertyLegalHistory = _propertyService.GetLegalHistory(PropertyFilterObj);
            return Json(propertyLegalHistory, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetPaymentDetails(int id)
        {
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            var propertyPaymentDetails = _propertyService.GetPaymentDetails(PropertyFilterObj);
            return Json(propertyPaymentDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPropertyAttachmentDetails(int id)
        {
            _propertyService = new PropertyService();
            var attachedDocumentPath = ConfigurationSettings.AppSettings["DocumentsPath"].ToString();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            IEnumerable<DocumentDetail> propertyDocumentDetail = _propertyService.GetPropertyAttachmentDetails(PropertyFilterObj);

            foreach (var item in propertyDocumentDetail)
            {
                item.PathName = attachedDocumentPath + item.PathName;
            }

            return Json(propertyDocumentDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentDetails(string id)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _id = 0;
            if (int.TryParse(id, out _id))
                PropertyFilterObj.RegistrationId = _id;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);
            FtpHandler ObjFtpHandler = new FtpHandler();
            string path = ObjFtpHandler.GetDirectoryPath(propDetails.DepttId, id, string.Empty, true);//System.Configuration.ConfigurationManager.AppSettings["RootPath"] + "\\" + id;
            List<string> allFilesOld = ObjFtpHandler.DirSearch(path);
            foreach (string file in allFilesOld)
            {
                string str = file;
                if (str.Contains(" "))
                {
                    str = str.Replace(" ", "");
                    ObjFtpHandler.RenameFiles(file, str, path);
                }
            }
            List<string> allFiles = ObjFtpHandler.DirSearch(path);
            List<DocumentDetail> lstDocumentDetail = new List<DocumentDetail>();
            foreach (string file in allFiles)
            {
                string str = file;
                string str1 = string.Empty;
                str1 = str.Substring(0, str.Length - 4);
                str1 = str1.Substring(9, str1.Length - 9);
                DocumentDetail objDocumentDetail = new DocumentDetail();
                objDocumentDetail.PathName = ObjFtpHandler.GetDirectoryPath(propDetails.DepttId, id, file, false);//"http://" + System.Configuration.ConfigurationManager.AppSettings["SiteRootPath"] + "//" + id + "//" + file;
                objDocumentDetail.DocumentName = System.Configuration.ConfigurationManager.AppSettings[str1];
                lstDocumentDetail.Add(objDocumentDetail);
            }
            return Json(lstDocumentDetail, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method returns all files in folder and subfolder. Not use in anymore as FTP is being used
        /// </summary>
        /// <param name="sDir"></param>
        /// <returns></returns>
        //private static List<String> DirSearch(string sDir)
        //{
        //    List<String> files = new List<String>();
        //    try
        //    {
        //        foreach (string f in Directory.GetFiles(sDir))
        //        {
        //            files.Add(f);
        //        }
        //        foreach (string d in Directory.GetDirectories(sDir))
        //        {
        //            files.AddRange(DirSearch(d));
        //        }
        //    }
        //    catch (System.Exception excpt)
        //    {
        //    }

        //    return files;
        //}

        public JsonResult GetPropertyDocuments(int id)
        {
            _propertyService = new PropertyService();
            IEnumerable<PropertyDocument> propertyLegalHistory = _propertyService.GetPropertyDocument(id);
            return Json(propertyLegalHistory, JsonRequestBehavior.AllowGet);
        }


        private PropertyDetail SetPropertyDetailObject(DtoPropertyDetails propDetails)
        {
            PropertyDetail propertyDetail = new PropertyDetail();
            // For Payment Schedule Popup[Add Noof Installmen. Total Amount Due, Total Amount paid]
            _propertyService = new PropertyService();
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            PropertyFilterObj.RegistrationId = propDetails.RegistrationId;
            DtoSchedulePaymentSummary propertyPaymentSchedule = _propertyService.GetPaymentScheduleSummary(PropertyFilterObj);
            if (propertyPaymentSchedule != null)
            {
                propertyDetail.NoOfInstallments = propertyPaymentSchedule.TotalEMICount;
                propertyDetail.TotalAmountDue = propertyPaymentSchedule.TotalDueAmount;
                propertyDetail.TotalAmountPaid = propertyPaymentSchedule.TotalPaidAmount;

            }

            // End

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

            if (!string.IsNullOrEmpty(propDetails.FunctionalDate))
            {
                propertyDetail.FunctionalDate = Convert.ToDateTime(propDetails.FunctionalDate).ToString("dd MMM yyyy");
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
            propertyDetail.PropertyNumber = propDetails.PropertyNumber + "-" + propDetails.Block;
            propertyDetail.PropertyCategory = propDetails.PropertyCategory;
            propertyDetail.TransferCase = propDetails.TransferCase;
            propertyDetail.ExtensionGiven = propDetails.ExtensionGiven;
            propertyDetail.JalConnection = propDetails.JalConnection;

            return propertyDetail;
        }

        private DtoPropertyFilter CreatePropertyFilter()
        {
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            if (Request.QueryString["pt"] != null)
                PropertyFilterObj.PropertyTypeId = Request.QueryString["pt"];
            if (Request.QueryString.ToString().Contains("DeptId="))
                PropertyFilterObj.PropertyTypeId = Request.QueryString.ToString().Last().ToString();
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
            if (Request.QueryString["pts"] != null)
                PropertyFilterObj.PaymentStatus = Convert.ToInt16(Request.QueryString["pts"]);
            if (Request.QueryString["cs"] != null)
                PropertyFilterObj.ComplaintStatusId = Convert.ToInt16(Request.QueryString["cs"]);
            if (Request.QueryString["qtr"] != null)
                PropertyFilterObj.SelectedQuarter = Convert.ToString(Request.QueryString["qtr"]);

            return PropertyFilterObj;
        }
        private DtoPropertyFilter CreatePropertyFilterForFinance()
        {
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            if (Request.QueryString["pt"] != null)
                PropertyFilterObj.PropertyTypeId = Request.QueryString["pt"];
            //if (Request.QueryString.ToString().Contains("DeptId="))
            //    PropertyFilterObj.PropertyTypeId = Request.QueryString.ToString().Last().ToString();
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
            if (Request.QueryString["pts"] != null)
                PropertyFilterObj.PaymentStatus = Convert.ToInt16(Request.QueryString["pts"]);
            if (Request.QueryString["cs"] != null)
                PropertyFilterObj.ComplaintStatusId = Convert.ToInt16(Request.QueryString["cs"]);
            if (Request.QueryString["qtr"] != null)
                PropertyFilterObj.SelectedQuarter = Convert.ToString(Request.QueryString["qtr"]);

            return PropertyFilterObj;
        }
        private string CreateFilterDisplayString(string prefiex)
        {
            StringBuilder PropertyFilterString = new StringBuilder();
            if (Request.QueryString["pt"] != null)
                PropertyFilterString.Append("Property Type:" + GetNameFromID(Contants.QueryString.PropertyType) + ", ");
            if (Request.QueryString["pa"] != null)
                PropertyFilterString.Append("Area:" + GetNameFromID(Contants.QueryString.PropertyArea) + ", ");
            if (Request.QueryString["ps"] != null)
                PropertyFilterString.Append("Allotment Status:" + GetNameFromID(Contants.QueryString.PropertyStatus) + ", ");
            if (Request.QueryString["yr"] != null)
                PropertyFilterString.Append("Year: " + Request.QueryString["yr"] + ", ");
            if (Request.QueryString["sc"] != null)
                PropertyFilterString.Append("Sector:" + Request.QueryString["sc"] + ", ");
            if (Request.QueryString["bk"] != null)
                PropertyFilterString.Append("Block:" + Request.QueryString["bk"] + ", ");
            if (Request.QueryString["pn"] != null)
                PropertyFilterString.Append("Property No:" + Request.QueryString["pn"] + ", ");
            if (Request.QueryString["pln"] != null)
                PropertyFilterString.Append("Plot No:" + Request.QueryString["pln"] + ", ");
            if (Request.QueryString["an"] != null)
                PropertyFilterString.Append("Allottee Name:" + Request.QueryString["an"] + ", ");
            if (Request.QueryString["st"] != null)
                PropertyFilterString.Append("Search Type:" + GetSearchType(Request.QueryString["st"]) + ", ");
            if (Request.QueryString["pts"] != null)
                PropertyFilterString.Append("Payment Type:" + GetPaymentStatus(Request.QueryString["pts"]) + ", ");
            if (Request.QueryString["id"] != null)
                PropertyFilterString.Append("Registration ID:" + Request.QueryString["id"] + ", ");
            if (Request.QueryString["qtr"] != null)
                PropertyFilterString.Append("Quarter:" + Request.QueryString["qtr"] + ", ");
            if (PropertyFilterString.Length > 0)
            {
                PropertyFilterString.Insert(0, prefiex + " for ");
            }
            else
                PropertyFilterString.Append(prefiex);
            return PropertyFilterString.ToString();
        }

        private string GetNameFromID(string name)
        {
            string id = Request.QueryString[name];
            _commonService = new CommonService();
            string displayName = string.Empty;
            DtoList list = null;
            switch (name)
            {
                case Contants.QueryString.PropertyType:
                    list = _commonService.GetPropertyType().Where(i => i.value == id).FirstOrDefault();
                    break;
                case Contants.QueryString.PropertyStatus:
                    list = _commonService.GetAlotmentStatus().Where(i => i.value == id).FirstOrDefault();
                    break;
                case Contants.QueryString.PropertyArea:
                    list = _commonService.GetAreaType().Where(i => i.value == id).FirstOrDefault();
                    break;
            }
            if (list != null)
                displayName = list.label;

            return displayName;
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
                case "4":
                    searchType = Contants.SearchType.Defaulter;
                    break;
            }
            return searchType;
        }
        // Get Payment Status
        private string GetPaymentStatus(string name)
        {
            string paymentType = string.Empty;
            switch (name)
            {
                case "1":
                    paymentType = Contants.PaymentType.Defaulted;
                    break;
                case "2":
                    paymentType = Contants.PaymentType.Due;
                    break;
                //case "3":
                //    paymentType = Contants.PaymentType.PartiallyPaid;
                //    break;
                //case "4":
                //    paymentType = Contants.PaymentType.Due;
                //    break;
            }
            return paymentType;
        }

        //[AllowAnonymous]
        //[HttpPost]
        public ActionResult ChallanPrint(int RegistrationId, int flag)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(RegistrationId), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);
            PropertyDetail ChallanPrintDetails = SetPropertyDetailObject(propDetails);
            if (flag == 0)
            { ChallanPrintDetails.TotalAmountDue = 0; }
            return View(ChallanPrintDetails);
        }

        public ActionResult ChallanPrintFullOrPartial(int RegistrationId, int flag, int partAmount)
        {
            _propertyService = new PropertyService();
            PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(RegistrationId), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;
            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);
            PropertyDetail ChallanPrintDetails = SetPropertyDetailObject(propDetails);
            if (flag == 0)
            { ChallanPrintDetails.TotalAmountDue = partAmount; }
            return View("ChallanPrint", ChallanPrintDetails);
        }

        public JsonResult SaveRemark(string registrationId, string remark)
        {
            _propertyService = new PropertyService();
            var flag = _propertyService.AddPropertyRemark(registrationId, remark);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remarks_Read(string id)
        {
            _propertyService = new PropertyService();
            var remarksLst = _propertyService.Remarks_Read(id);
            return Json(remarksLst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllServiceData(string deptt)
        {
            _commonService = new CommonService();
            var list = _commonService.GetAllServiceData(Convert.ToInt32(deptt));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VacantProperties()
        {
            PropertyDetail propertyDetail = new PropertyDetail();
            return View(propertyDetail);
        }

        public JsonResult GetServiceRequestReport([DataSourceRequest] DataSourceRequest request, int? departmentId, DateTime? fromDate, DateTime? toDate, int? serviceId, string registrationId)
        {
            _propertyService = new PropertyService();
            DataSourceResult data = _propertyService.GetServiceRequestReport(request, departmentId, fromDate, toDate, serviceId, registrationId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceRequestForUnRegisteredProperty([DataSourceRequest] DataSourceRequest request)
        {
            _propertyService = new PropertyService();
            DataSourceResult data = _propertyService.GetServiceRequestForUnRegisteredProperty(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetServiceRequestDataAsDataSource([DataSourceRequest] DataSourceRequest request, ServiceViewModel model)
        {
            //_propertyService = new PropertyService();
            //var rsltLst = _propertyService.SR_Read(request);
            _requestService = new RequestService();
            var list = _requestService.GetServiceRequestDataAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetApplicantDetailsByRegistrationId(int? registrationId)
        {
            var detail = _requestService.GetApplicantDetailsByRegistrationId(registrationId);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFileUploadOptionsForService(int? departmentId, int? serviceId)
        {
            var checkList = _requestService.GetFileUploadHtmlForService(departmentId, serviceId);
            if (checkList != null) return Json(checkList, JsonRequestBehavior.AllowGet);
            else return Json("NotExist", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ServiceRequest(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            if (model != null)
            {
                ServiceRequestViewModel service = _requestService.SaveServiceRequestDetail(model, files);
                //ServiceRequestViewModel service = _requestService.SaveServiceRequestForSamadhanDiwas(model, files);
                return Json(service, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public JsonResult SaveDirectorOrShareholders(string directorName, decimal? share, string shareType)
        {
            var shares = _requestService.SaveDirectorOrShareholders(directorName, share, shareType);
            return Json(shares, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDirectorShareholderDetails([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["TempDirectors"] != null)
            {
                var directorList = (List<DirectorShareholderModel>)Session["TempDirectors"];
                return Json(directorList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveDirectorShareholderDetail(int id)
        {
            var flag = false;
            var directorList = (List<DirectorShareholderModel>)Session["TempDirectors"];
            if (directorList != null)
            {
                directorList.RemoveAt(id - 1);
                flag = true;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMortgageTypeList()
        {
            var typeList = _commonService.GetMortgageTypeList();
            return Json(typeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNOCStatusList()
        {
            var typeList = _commonService.GetNOCStatusList();
            return Json(typeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferTypeList()
        {
            var transferList = _commonService.GetTransferTypeList();
            return Json(transferList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferSubTypeList(int transferTypeId)
        {
            var transferSubList = _commonService.GetTransferSubTypeList(transferTypeId);
            return Json(transferSubList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGenderList()
        {
            var gender = _commonService.GetGenderList();
            return Json(gender, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOccupationList()
        {
            var occupation = _commonService.GetOccupationList();
            return Json(occupation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCICRequestTypeList()
        {
            var occupation = _commonService.GetCICRequestTypeList();
            return Json(occupation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentList()
        {
            var department = _commonService.GetDepartmentListII();
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceListByDepartment(int departmentId)
        {
            var services = _commonService.GetServiceListByDepartment(departmentId);
            return Json(services, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubDepartmentList(int departmentId)
        {
            var department = _commonService.GetSubDepartmentList(departmentId);
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyMemberTypeList()
        {
            var typeList = _commonService.GetCompanyMemberTypeList();
            return Json(typeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFirmStatusList()
        {
            var typeList = _commonService.GetFirmStatusList();
            return Json(typeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGPAStatusList()
        {
            var statusList = _commonService.GetGPAStatusList();
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceRequestReportAsDataSource([DataSourceRequest] DataSourceRequest request, ServiceViewModel model)
        {
            DataSourceResult data = _requestService.GetServiceRequestReportAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }        

        public JsonResult GetTransferHistoryByIdAsDataSource([DataSourceRequest]DataSourceRequest request, TransferViewModel model)
        {
            var data = _customerService.GetTransferHistoryByIdAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMortgageHistoryByIdAsDataSource([DataSourceRequest]DataSourceRequest request, MortgageViewModel model)
        {
            var data = _customerService.GetMortgageHistoryByIdAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExtensionHistoryByIdAsDataSource([DataSourceRequest]DataSourceRequest request, ExtensionViewModel model)
        {
            var data = _customerService.GetExtensionHistoryByIdAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRentingHistoryByIdAsDataSource([DataSourceRequest]DataSourceRequest request, RentingViewModel model)
        {
            var data = _customerService.GetRentingHistoryByIdAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCICHistoryByIdAsDataSource([DataSourceRequest]DataSourceRequest request, CICViewModel model)
        {
            var data = _customerService.GetCICHistoryByIdAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFunctionalHistoryByIdAsDataSource([DataSourceRequest]DataSourceRequest request, FunctionalViewModel model)
        {
            var data = _customerService.GetFunctionalHistoryByIdAsDataSource(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}