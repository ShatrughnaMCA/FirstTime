using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoidaAuthority.PMS.Entity.NaUser;
using System.Configuration;
using System.IO;
using NoidaAuthority.PMS.Service.ManageUsers;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Common.Logger;


namespace NoidaAuthority.PMS.Web.Areas.Customer.Controllers
{
    public class PropertyController : Controller
    {
        private ICustomerService _customerService;
        private ICommonService _commonService;
        private IRequestService _requestService;
        private IManageCitizenService _manageCitizenService;
        private IManageUserService manageUserService;
        private ILogService logService = null;
        IPropertyService _propertyService;
        

        public PropertyController(ICustomerService customerService, ICommonService commonService, IRequestService requestService)
        {
            _customerService = customerService;
            _commonService = commonService;
            _requestService = requestService;
            _manageCitizenService = new ManageCitizenService();
            manageUserService = new ManageUserService();
            logService = new LogService();
            _propertyService = new PropertyService();
        }

        public int GetRegistrationId()
        {
            int rid = 0;
            //if (Session["CurrentCustomer"] != null)
            //{
            //    var user = (CustomerViewModel)Session["CurrentCustomer"];
            //    rid = Convert.ToInt32(user.UserName);
            //}
            if (Session["RegistrationId"] != null)
            {
                var id = Session["RegistrationId"];
                rid = Convert.ToInt32(id);
            }
            return rid;
        }

        // GET: Customer/Property
        public ActionResult Index()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult Dashboard()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult Documents()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult Formats()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult Litigation()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult Payment()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                var ndcdetails = _customerService.GetNDCDetailsByRid(rid);
                data.PaymentModel = new PaymentViewModel();
                data.PaymentModel.IsOneTimeLeaseRentPaid = ndcdetails.IsOneTimeLeaseRentPaid;
                data.PaymentModel.IsTotalInstallmentPaid = ndcdetails.IsTotalInstallmentPaid;
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area=""});
            }
        }

        public ActionResult Services()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area=""});
            }
        }

        public ActionResult ServiceRequest(string id)
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var propertyDetail = _customerService.GetPropertyDetailForServiceRequestById(rid);
                return View(propertyDetail);
            }
            else
            {
                //rid = !string.IsNullOrEmpty(id) ? Convert.ToInt32(CommonHelper.Decode(id)) : 10000013;
                //var propertyDetail = _customerService.GetPropertyDetailForServiceRequestById(rid);
                return RedirectToAction("Logout", "Account", new { area = "" });
            } 
        }

        [HttpPost]
        public ActionResult ServiceRequest(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var detail = _customerService.SaveServiceRequestDetail(model, files);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceDetail(string id)
        {
            int sid = !string.IsNullOrEmpty(id) ? Convert.ToInt32(CommonHelper.Decode(id)) : 10000013;
            var details = _customerService.GetServiceRequestDetailById(sid);
            return View(details);
        }

        public ActionResult History()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var model = _customerService.GetCustomerDetails(rid);
                return View(model);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult Notices()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult NoidaJal()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public ActionResult OnlineHelp()
        {
            int rid = GetRegistrationId();
            if (rid != 0)
            {
                var data = _customerService.GetCustomerDetails(rid);
                return View(data);
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
        }

        public JsonResult GetScannedDocumentListById([DataSourceRequest]DataSourceRequest request, DocumentViewModel model)
        {
            var data = _customerService.GetScannedDocumentListById(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGeneratedDocumentListById([DataSourceRequest]DataSourceRequest request, DocumentViewModel model)
        {
            var data = _customerService.GetGeneratedDocumentListById(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentReceiptScheduleListById([DataSourceRequest]DataSourceRequest request, PaymentViewModel model)
        {
            var data = _customerService.GetPaymentReceiptScheduleListById(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentScheduleDataListById([DataSourceRequest]DataSourceRequest request, int? rid)
        {
            var data = _customerService.GetPaymentScheduleDataListById(request, rid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentLedgerDataListById([DataSourceRequest]DataSourceRequest request, int? rid)
        {
            var data = _customerService.GetPaymentLedgerDataListById(request, rid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceHistoryDataListById([DataSourceRequest]DataSourceRequest request, ServiceViewModel model)
        {
            var data = _customerService.GetServiceHistoryDataListById(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //today
        public JsonResult GetServiceListByDepartment(int departmentId)
        {
            var data = _commonService.GetServiceListByDepartment(departmentId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubDepartmentList(int departmentId)
        {
            var data = _commonService.GetSubDepartmentList(departmentId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferTypeList()
        {
            var data = _commonService.GetTransferTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferSubTypeList(int transferTypeId)
        {
            var data = _commonService.GetTransferSubTypeList(transferTypeId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGenderList()
        {
            var data = _commonService.GetGenderList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOccupationList()
        {
            var data = _commonService.GetOccupationList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCICRequestTypeList()
        {
            var data = _commonService.GetCICRequestTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyMemberTypeList()
        {
            var data = _commonService.GetCompanyMemberTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFirmStatusList()
        {
            var data = _commonService.GetFirmStatusList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMortgageTypeList()
        {
            var data = _commonService.GetMortgageTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGPAStatusList()
        {
            var data = _commonService.GetGPAStatusList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDirectorShareholderDataList(DataSourceRequest request)
        {
            var data = _customerService.GetDirectorShareholderDataList(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDirectorOrShareholders(string directorName, decimal? share, string shareType)
        {
            var flag = _customerService.SaveDirectorOrShareholders(directorName, share, shareType);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveDirectorShareholderFromList(int id)
        {
            var flag = _customerService.RemoveDirectorShareholderFromList(id);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFileUploadOptionsForService(int? departmentId, int? serviceId)
        {
            var checkList = _customerService.GetFileUploadHtmlForService(departmentId, serviceId);
            if (checkList != null) return Json(checkList, JsonRequestBehavior.AllowGet);
            else return Json("NotExist", JsonRequestBehavior.AllowGet);
        }       

        public JsonResult GetTransferHistoryDataListById(DataSourceRequest request, int? rid)
        {
            var data = _customerService.GetTransferHistoryDataListById(request, rid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMortgageHistoryDataListById(DataSourceRequest request, int? rid)
        {
            var data = _customerService.GetMortgageHistoryDataListById(request, rid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExtensionHistoryDataListById(DataSourceRequest request, int? rid)
        {
            var data = _customerService.GetExtensionHistoryDataListById(request, rid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJalPaymentDataList([DataSourceRequest]DataSourceRequest request, JalViewModel model)
        {
            //var list = _customerService.GetJalPaymentDataList(request, model);
            var list = _customerService.GetJalPaymentPISDataList(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLitigationDataList([DataSourceRequest]DataSourceRequest request, LitigationViewModel model)
        {
            //var list = _customerService.GetLitigationDataList(request, model);
            var list = _customerService.GetLitigationPISDataList(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMultiplePaymentStatusList([DataSourceRequest] DataSourceRequest request, PaymentViewModel model)
        {
            var list = _customerService.GetMultiplePaymentStatusList(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInstallmentPaymentDues([DataSourceRequest] DataSourceRequest request, PaymentViewModel model)
        {
            var list = _customerService.GetInstallmentPaymentDues(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNDCDetails([DataSourceRequest] DataSourceRequest request, PaymentViewModel model)
        {
            var list = _customerService.GetNDCDetails(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNoticesAndRemarksAsDataSource([DataSourceRequest] DataSourceRequest request, CustomerViewModel model)
        {
            var list = _customerService.GetNoticesAndRemarksAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNOCStatusList()
        {
            var typeList = _commonService.GetNOCStatusList();
            return Json(typeList, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult LetterVerification()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ServiceStatus()
        {
            CitizenServiceRequest model = new CitizenServiceRequest();
            return View(model);
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


        //To get all banks in ddl
        public JsonResult GetAllBanks()
        {
            var lst = _customerService.GetAllBanks();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //To get all branches in ddl
        public JsonResult GetAllBranchs(int bankId)
        {
            var lst = _customerService.GetAllBranchs(bankId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //To get Account Number base on bankid and branchid
        public JsonResult GetAccountNumber(int bankId, int branchId)
        {
            var data = _customerService.GetAccountNumber(bankId, branchId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //To get all branches in ddl
        public JsonResult GetAllAccountHead()
        {
            var lst = _customerService.GetAllAccountHead();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //To get all Account Sub Head base on accountId
        public JsonResult GetAccountSubHead(int AccountHeadId)
        {
            var lst = _customerService.GetAccountSubHead(AccountHeadId);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        //Save challan detils in db condider paramenter rid,accountHeadId,AccountSubHeadId,Amount
        public JsonResult SaveCreateChallan(int rId, int AccountHeadId, int AccountSubHeadId, decimal? Amount)
        {
            var flag = _customerService.SaveCreateChallan(rId, AccountHeadId, AccountSubHeadId, Amount);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetGeneratedChallanDetails([DataSourceRequest] DataSourceRequest request, int rid)
        //{
        //    List<PaymentViewModel> dataresult = _customerService.GetGeneratedChallanDetails(rid);
        //    if (dataresult == null)
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var data = dataresult.ToDataSourceResult(request);
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }

        //}

        public JsonResult GetServiceRequestDocumentsById([DataSourceRequest]DataSourceRequest request, ServiceViewModel model)
        {
            var reports = _customerService.GetServiceRequestUploadedDocumentsById(request, model);
            return Json(reports, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRaisedServiceRequestStatusById(ServiceViewModel model)
        {
            int flag = _requestService.GetRaisedServiceRequestStatusById(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReSubmitRequest(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            //var detail = _customerService.SaveServiceRequestDetail(model, files);
            var detail = _customerService.ReSubmitServiceRequestDetail(model, files);
            return Json(detail.ServiceModel.ActionTypeId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateServiceRequestStatus(ActionViewModel model)
        {
            int flag = _requestService.UpdateServiceRequestStatus(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUploadedDocumentByServiceId(int Id, int Rid, string ActionType)
        {
            var reports = _customerService.GetUploadedDocumentByServiceId(Id, Rid, ActionType);
            return Json(reports, JsonRequestBehavior.AllowGet);
        }

    }
}