using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Service.Admin;
using NoidaAuthority.PMS.Service.ManageUsers;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Web.Filters;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Common;
using System.Configuration;
using System.IO;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Web.Models;
//using CaptchaMvc.HtmlHelpers;
//using CaptchaMvc.Attributes;

namespace NoidaAuthority.PMS.Web.Controllers.ManageUsers
{
    public class ManageUsersController : WebBaseController
    {

        IManageUserService _userService = null;
        IManageRoleService ManageRoleService { get; set; }
        private ILogService logService = null;
        IPropertyService _propertyService;
        DtoPropertyFilter PropertyFilterObj;
        ICommonService _commonService = null;


        public ManageUsersController(IManageUserService userService, IManageRoleService manageRoleService)
        {
            _userService = new ManageUserService(); // userService;
            this.ManageRoleService = manageRoleService;
            _commonService = new CommonService();
        }

        public ActionResult ManageUsers()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CustomerBackToLogin()
        {
            return View();
        }

        [AjaxHandleException]
        public ActionResult ManageUsersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var userList = _userService.GetUsers();
            userList = userList.Where(ul => ul.RoleName.ToLower() == NoidaAuthority.PMS.Common.Contants.Roles.Administrator.ToString().ToLower()).OrderBy(ul => ul.Status);
            DataSourceResult result = userList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult ManageCustomersAjax([DataSourceRequest] DataSourceRequest request)
        {
            var userList = _userService.GetUsers();
            userList = userList.Where(ul => ul.RoleName.ToLower() == NoidaAuthority.PMS.Common.Contants.Roles.Customer.ToString().ToLower()).OrderByDescending(ul => ul.CreatedOn);
            DataSourceResult result = userList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AjaxHandleException]
        public ActionResult ManageRoleAjax([DataSourceRequest] DataSourceRequest request)
        {
            var manageRoles = ManageRoleService.GetRoles();
            manageRoles.RoleList = manageRoles.RoleList.Where(x => x.RoleName.ToLower() != "customer").ToList();

            var roles = (from x in manageRoles.RoleList
                         select new { RoleId = x.RoleId, RoleName = x.RoleName }).ToList();
            return Json(roles, JsonRequestBehavior.AllowGet);

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

        public ActionResult EditAdmin(Guid uid)
        {
            ViewBag.Title = "Edit Administrator";
            var model = new NaUser();
            if (uid != new Guid())
            {
                model = _userService.GetUserById(uid);
                ViewBag.isUpdateMode = true;
            }
            else
            {
                model.Status = true;
            }
            return View("AddEditAdmin", model);
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

        //added by Shatrughna
        [AllowAnonymous]
        public ActionResult AddEditNAcustomer()
        {
            ViewBag.Title = "Update Customer";
            var model = new NAcustomer();
            model.RandomKeyVal = RandomString(7);
            model.Status = true;
            return View(model);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAdmin(NaUser model)
        {
            var result = false;
            model.Status = Contants.status;
            if (model.id == 1)
            {
                model.RoleId = Contants.AdminRoleId;
                model.DeptId = 0;
            }
            else
                model.RoleId = Contants.DeptRoleId;

            var isNew = model.UserId != new Guid() ? false : true;
            if (Session["CurrentUser"] != null)
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                model.CreatedBy = currentUser.UserId;

                result = _userService.AddEditUser(model);
            }
            // set notification mode
            if (result)
            {
                if (isNew)
                    TempData["isCreated"] = true;
                else
                {
                    TempData["isUpdated"] = true;
                }
            }

            return RedirectToAction("ManageUsers");
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(NaUser model)
        {
            model.Status = Contants.status;
            model.RoleId = Contants.BackOfficeRoleId;
            model.DeptId = -2;
            var result = false;
            var isNew = model.UserId != new Guid() ? false : true;
            ModelState.Remove("PropertyId");

            if (model.UserId != new Guid()) // in case of Edit user
                ModelState.Remove("Email");

            //ModelState.Remove("Department");
            //ModelState.Remove("Notes");


            if (ModelState.IsValid)
            {
                if (Session["CurrentUser"] != null)
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    model.CreatedBy = currentUser.UserId;

                    result = _userService.AddEditUser(model);
                }
                else
                {
                    model.CreatedBy = new Guid(Contants.UserId);
                    result = _userService.AddEditUser(model);
                }

            }
            // set notification mode
            if (result)
            {
                if (!isNew)
                {
                    TempData["isUpdated"] = true;
                }
                else
                {
                    TempData["isCreated"] = true;

                }

            }
            // Updated changes for bug #6156z

            return RedirectToAction("ManageUsers");
        }

        //added by Shatrughna

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RegisterNAcustomer(NAcustomer model, HttpPostedFileBase upload1, HttpPostedFileBase upload2)
        {
            var result = false;
            model.Status = Contants.status;

            //var isNew = model.CustomerId != new Guid() ? false : true;


            if (ModelState.IsValid)
            {
                if (upload1 != null && upload1.ContentLength > 0)
                {
                    if (model.CustomerIdFiletype.Equals("PANCard"))
                    {
                        //model.CustomerIdFileName = "PC-" + upload1.FileName;
                        model.CustomerIdFileName = upload1.FileName;
                    }
                    if (model.CustomerIdFiletype.Equals("AadhaarCard"))
                    {
                        //model.CustomerIdFileName = "AC-" + upload1.FileName;
                        model.CustomerIdFileName = upload1.FileName;
                    }
                }
                if (upload2 != null && upload2.ContentLength > 0)
                {
                    if (model.AuthorityLetterType.Equals("AllotmentLetter"))
                    {
                        //model.AuthorityLetter = "AL-" + upload2.FileName;
                        model.AuthorityLetter = upload2.FileName;
                    }
                    if (model.AuthorityLetterType.Equals("TransferLetter"))
                    {
                        //model.AuthorityLetter = "TL-" + upload2.FileName;
                        model.AuthorityLetter = upload2.FileName;
                    }
                }
                result = _userService.AddEditNAcustomer(model);
                if (!Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId))
                {
                    Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId);
                    upload1.SaveAs(Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + upload1.FileName);
                    if (upload2 != null)
                        upload2.SaveAs(Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + upload2.FileName);
                }
                else
                {
                    upload1.SaveAs(Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + upload1.FileName);
                    if (upload2 != null)
                        upload2.SaveAs(Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + upload2.FileName);

                }

            }

            if (result)
            {
                TempData["isUpdated"] = true;
            }

            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        TempData["CaptchaError"] = "CaptchaError";

                    }
                }

            }
            //else
            //{
            //    TempData["isCreated"] = true;
            //    TempData["isCreated"] = true;
            //}

            // Updated changes for bug #6156z
            return RedirectToAction("CustomerBackToLogin");

            //return View("RegisterMessage");
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendWelcomeEmail(NaUser model)
        {
            var result = false;

            //ModelState.Remove("Department");
            //ModelState.Remove("Notes");
            if (ModelState.IsValid)
            {
                if (Session["CurrentUser"] != null)
                {
                    var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                    model.CreatedBy = currentUser.UserId;

                    result = _userService.AddEditUser(model);
                }

            }
            // set notification mode
            if (result)
            {

                TempData["isUpdated"] = true;

            }
            // Updated changes for bug #6156
            return RedirectToAction("ManageUsers");
        }


        public ActionResult VerifyEmail(string email)
        {
            var result = false;
            var users = _userService.GetUsers();

            var tier = users.FirstOrDefault(ur => ur.UserName.ToLower() == email.ToLower());
            if (tier != null)
            {
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);



        }

        public JsonResult LockUnLockCustomer(string email)
        {
            var flag = false;
            flag = _userService.LockUnLockCustomer(email);
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

        public JsonResult RejectCustomer(string email, string mobileNo, string remarks)
        {
            var flag = false;
            flag = _userService.RejectCustomer(email, mobileNo, remarks);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendPassword(ActionViewModel model)
        {
            var flag = _userService.SendPassword(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        //  [HttpPost, CaptchaVerify("Captcha is not valid")]
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

        [AllowAnonymous]
        public JsonResult CheckRegistationIDForNAcustomer(String RegistrationId)
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
        

        public ActionResult Feedbacks()
        {
            return View();
        }

        public JsonResult TakeActionOnUserByType(ActionViewModel model)
        {
            int flag = _userService.TakeActionOnUserByType(model);
            return Json(flag,JsonRequestBehavior.AllowGet);
        }
    }
}