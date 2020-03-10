using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Service.Admin;
using NoidaAuthority.PMS.Service.ManageUsers;
using NoidaAuthority.PMS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Common.Logger;
using System.Configuration;
using System.IO;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Common.Helpers;

namespace NoidaAuthority.PMS.Web.Areas.Administration.Controllers
{
    public class AdminController : Controller
    {
        IManageUserService _userService;
        IManageRoleService ManageRoleService { get; set; }
        IPropertyService _propertyService;
        DtoPropertyFilter PropertyFilterObj;
        ICommonService _commonService;

        private ILogService logService = null;

        public AdminController(IManageUserService userService, IManageRoleService manageRoleService, IPropertyService propertyService, ICommonService commonService)
        {
            _userService = userService;
            this.ManageRoleService = manageRoleService;
            _propertyService = new PropertyService();
            _commonService = new CommonService();
        }

        // GET: Administration/Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            CommonService cms = new CommonService();
            var pCount = cms.GetPropertyCount();
            ViewData["PropCount"] = pCount;
            return View();
        }

        public ActionResult ManageCustomer()
        {
            return View();
        }

        public ActionResult ManageEmployee()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Employee()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public JsonResult GetRegisteredCustomerList([DataSourceRequest] DataSourceRequest request, UserViewModel model)
        {
            var list = _userService.GetRegisteredCustomerList(request, model);
            //return Json(list, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetRegisteredEmployeeList([DataSourceRequest] DataSourceRequest request,UserViewModel model)
        {
            var data = _userService.GetRegisteredEmployeeList(request, model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult ManageUsersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var userList = _userService.GetUsers();
            userList = userList.Where(ul => ul.RoleName.ToLower() == NoidaAuthority.PMS.Common.Contants.Roles.Administrator.ToString().ToLower()).OrderBy(ul => ul.Status);
            DataSourceResult result = userList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult AddEditCustomer()
        {
            ViewBag.Title = "Add New Customer";
            var model = new NaUser();
            //if (uid != new Guid())
            //{
            //    Session["UserUid"] = uid;
            //    model = _userService.GetUserById(uid);
            //    ViewBag.isUpdateMode = true;

            //}


            model.Status = true;
            model.Email = "";
            model.Password = "";


            return View(model);
        }

        public ActionResult AddEditAdmin(Guid uid)
        {
            ViewBag.Title = "Add New Administrator";
            var model = new NaUser();
            if (uid != new Guid())
            {
                model = _userService.GetUserById(uid);
                ViewBag.isUpdateMode = true;
            }
            else
            {
                ViewBag.RolesLst = ManageRoleService.GetRolesForDdl();

                model.Status = true;
            }

            return View(model);
        }

        public ActionResult EditCustomer(Guid uid)
        {
            ViewBag.Title = "Edit Customer";
            var model = new NaUser();
            if (uid != new Guid())
            {
                Session["UserUid"] = uid;
                model = _userService.GetUserById(uid);
                ViewBag.isUpdateMode = true;

            }
            return View("AddEditCustomer", model);
        }

        public JsonResult LockUnLockCustomer(string email)
        {
            var flag = false;
            flag = _userService.LockUnLockCustomer(email);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendPassword(ActionViewModel model)
        {
            int flag = _userService.SendPassword(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeactivateUser(string email)
        {
            var flag = false;
            flag = _userService.DeactivateUser(email);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPassword(string email)
        {
            var flag = false;
            flag = _userService.ResetPassword(email);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult ManageCustomersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var userList = _userService.GetUsers();
            userList = userList.Where(ul => ul.RoleName.ToLower() == NoidaAuthority.PMS.Common.Contants.Roles.Customer.ToString().ToLower()).OrderByDescending(ul => ul.CreatedOn);
            DataSourceResult result = userList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RejectCustomer(string email, string mobileNo, string remarks)
        {
            var flag = false;
            flag = _userService.RejectCustomer(email, mobileNo, remarks);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }



        //public JsonResult GetProperties([DataSourceRequest] DataSourceRequest req, List<string> options)
        //{
        //    _propertyService = new PropertyService();
        //    PropertyFilterObj = CreatePropertyFilter();
        //    if (Session["DeptId"] != null)
        //    {
        //        if (!string.IsNullOrEmpty(Request.QueryString["pt"]))
        //        {
        //            PropertyFilterObj.PropertyTypeId = Request.QueryString["pt"];
        //        }
        //        else if (Session["DeptId"].ToString() == "0")
        //            PropertyFilterObj.PropertyTypeId = null;
        //        else
        //            PropertyFilterObj.PropertyTypeId = Session["DeptId"].ToString();
        //    }
        //    var propertyList = _propertyService.GetPropertyList(PropertyFilterObj, req);
        //    var wrapped = new { data = propertyList.Data, total = propertyList.Total };
        //    //foreach (DtoProperty objDtoProperty in propertyList)
        //    //{
        //    //    objDtoProperty.Block = objDtoProperty.Block.Trim();
        //    //}
        //    //Following added to resolve the exception "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property."
        //    return Json(wrapped, JsonRequestBehavior.AllowGet);
        //    //return new JsonResult()
        //    //{
        //    //    Data = propertyList,
        //    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //    //    MaxJsonLength = Int32.MaxValue
        //    //};
        //}

        public JsonResult GetSecurityQuestionList()
        {
            var data = _commonService.GetSecurityQuestionList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerIdList()
        {
            var data = _commonService.GetCustomerIdList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLetterTypeList()
        {
            var data = _commonService.GetLetterTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegisterNAEmployee(UserViewModel model)
        {
            var data = _userService.SaveNoidaEmployee(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartmentList()
        {
            var department = _commonService.GetDepartmentListII();
            return Json(department, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRoleTypeList()
        {
            var department = _commonService.GetRoleTypeList();
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserNameExist(string userName)
        {
            var flag = _userService.IsUserNameExist(userName);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RegisterCustomer(UserViewModel model, HttpPostedFileBase fileI, HttpPostedFileBase fileII)
        {
            int flag = _userService.SaveNACustomerDetails(model, fileI, fileII);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult PropertyDetailByRId(int id)
        {
            _propertyService = new PropertyService();
            var PropertyFilterObj = new DtoPropertyFilter();
            int _RegistrationId = 0;
            if (int.TryParse(Convert.ToString(id), out _RegistrationId))
                PropertyFilterObj.RegistrationId = _RegistrationId;

            DtoPropertyDetails propDetails = _propertyService.GetPropertyDetails(PropertyFilterObj, 1);

            return Json(propDetails);
        }

        [AllowAnonymous]
        public JsonResult CheckEmailForNAcustomerRegistation(String emailId)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = validateLogin.CheckEmailAddressForNAcustomer(emailId);
            }
            catch (Exception ex)
            {
                logService.LogError("Error in CheckEmailAddressForNAcustomerRegistation", ex);
            }
            return Json(flag);
        }

        [AllowAnonymous]
        public JsonResult GenerateOTP(string mobNo)
        {
            var flag = false;
            int r = 0;

            if (ModelState.IsValid)
            {
                Session["OTP"] = null;

                Random random = new Random();
                int maxValue = 999999;
                r = random.Next(maxValue);
                if (r != 0)
                {
                    flag = _userService.sendOTPtoUser(mobNo, r);
                }

                //return Json(flag, JsonRequestBehavior.AllowGet);

            }
            return Json(new { flag = flag, OTPValue = r }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult TakeActionOnUserByType(ActionViewModel model)
        {
            int flag = _userService.TakeActionOnUserByType(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSectorList(DropdownViewModel model)
        {
            var sectors = _commonService.GetSectorList(model);
            return Json(sectors, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBlockList(DropdownViewModel model)
        {
            var sectors = _commonService.GetBlockList(model);
            return Json(sectors, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateRegistrationId(PropertyViewModel model)
        {
            PropertyViewModel data = _userService.ValidateRegistrationId(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentPath(DocumentViewModel model)
        {
            FtpHandler ftp = new FtpHandler();
            //string path = ftp.GetDocumentPath(model.RegistrationId, string.Empty, true);
            string path = "D:/ProdPIS/NST_PROD_PIS/Code/NoidaAuthority.PMS-UI/NoidaAuthority.PMS.Web/Documents/10001676/ApprovedSitePlan.pdf";
            //List<string> allFiles = ftp.DirSearch(path);
            //string fileName = string.Empty;
            //if (allFiles != null && allFiles.Count > 0)
            //{
            //    if (model.ActionId == 1)
            //    {
            //        var file = allFiles.Count > 0 ? allFiles[0] : null;
            //        if (file != null) fileName = path; //FtpHandler.GetDocumentPathForKYA(model.RegistrationId.ToString(), Constants.KYA, file, false);
            //    }
            //    if (model.ActionId == 2)
            //    {
            //        var file = allFiles.Count > 1 ? allFiles[1] : null;
            //        if (file != null) fileName = path;//FtpHandler.GetDocumentPathForKYA(model.RegistrationId.ToString(), Constants.KYA, file, false);
            //    }
            //    if (model.ActionId == 3)
            //    {
            //        if (allFiles.Count > 2)
            //        {
            //            var file = allFiles[2];
            //            if (file != null) fileName = path;//FtpHandler.GetDocumentPathForKYA(model.RegistrationId.ToString(), Constants.KYA, file, false);
            //        }
            //        else fileName = null;// string.Empty;
            //    }
            //}
            return Json(path, JsonRequestBehavior.AllowGet);
        }
    }
}