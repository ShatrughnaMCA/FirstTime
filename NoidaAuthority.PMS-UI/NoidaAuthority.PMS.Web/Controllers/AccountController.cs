using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Service.Admin;
using NoidaAuthority.PMS.Service.Login;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Web.Models;
using NoidaAuthority.PMS.Common.EmailTemplate;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Service.ManageUsers;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Web.Controllers
{
    public class AccountController : Controller //WebBaseController
    {
        IManageUserService _userService;
        IManageRoleService ManageRoleService { get; set; }


        ICommonService _commonService;
        ILogin _loginService = null;
        private ILogService logService = null;
        private IManageRoleService _manageRoleService = null;

        public AccountController(LoginService login, ILogService log, IManageRoleService manageRole)
        {
            _loginService = login;
            logService = log;
            _manageRoleService = manageRole;
            _commonService = new CommonService();
            _userService = new ManageUserService();

        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        
        {
            //if (Session["allMenudIdsForRoleUser"] != null && Session["CurrentUser"] != null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            if (Session["UserDetail"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                if (Session["TempFrgtMsg"] != null)
                {
                    TempData["isForgotPwdSuccess"] = true;
                    Session["TempFrgtMsg"] = null;
                }
                Session["isForgotPwdSuccess"] = null;
                return View();
            }
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                Session["allMenudIdsForRoleUser"] = null;
                int flag = _loginService.ValidateUser(new UserViewModel { UserName = model.UserName, Password=model.Password.ToMD5HashForPassword() });
                if (flag == ReturnType.Success)
                {
                    Session["AttemptCount"] = null;
                    var UserDetail = _loginService.GetAuthorityUserDetail(model);
                    if (UserDetail.IsLocked == false || UserDetail.IsLocked == null)
                    {
                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(UserDetail);
                        Session["UserDetail"] = UserDetail;
                        Session["RoleId"] = UserDetail.RoleId;
                        Session["RegistrationId"] = UserDetail.RegistrationId;

                        var authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), model.IsActive.Value, userData, FormsAuthentication.FormsCookiePath);
                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);

                        if (UserDetail.RoleId == 0)
                        {
                            return Redirect("/Administration/Admin/ManageEmployee");
                        }
                        else if (UserDetail.RoleId == 1)
                        {
                            //return Redirect("/Administration/Property/Dashboard");
                            return RedirectToAction("Dashboard", "Property", new { area = "Administration" });
                        }
                        else if (UserDetail.RoleId == 2)
                        {
                            return Redirect("/Customer/Property/Dashboard");
                        }
                        
                        //var customUrl = "/Customer/Property/Dashboard";

                        //if (Session["RoleId"].Equals("0") || Session["RoleId"].Equals("1"))
                        //{
                        //    return RedirectToLocal(returnUrl);
                        //}
                        //else
                        //{
                        //    return Redirect(customUrl);
                        //}
                    }
                    else
                    {
                        if (UserDetail.IsLocked==true)
                            ModelState.AddModelError("ErrorMessage", "Your Account has been locked, Please Contact Administrator.");
                        if (UserDetail.IsActive == false)
                            ModelState.AddModelError("ErrorMessage", "User is inactive.");
                    }
                }
                else
                {

                    if (Session["AttemptCount"] == null) Session["AttemptCount"] = 1;
                    else
                    {
                        var attempts = (Int32)Session["AttemptCount"];
                        if (attempts == 5) attempts = 5;
                        else ++attempts;

                        if (attempts == 4) ModelState.AddModelError("ErrorMessage", "This is your last attempt to enter username/password.");                        
                        if (attempts == 5)
                        {
                            attempts = 5;
                            int lockflag = _loginService.LockApplicationUser(model);
                            if (lockflag == ReturnType.Locked) ModelState.AddModelError("ErrorMessage", "Your account has been locked, please contact Administrator.");
                        }
                        Session["AttemptCount"] = attempts;
                    }
                    ModelState.AddModelError("ErrorMessage", "Invalid username or password.");
                }
            }
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserDetail"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout", "Account", new { area = "" });
            }
            //ViewData["Message"] = message;       
            //passwordModel.OldPassword = "";
            //passwordModel.NewPassword = "";
            //passwordModel.ConfirmPassword = "";
            
        }

        public ActionResult UpdatePassword(ManageUserViewModel passwordModel)
        {
            var flag = false;
            int? regID = 0;
            var userName = string.Empty;
            var currentUser = (UserViewModel)Session["UserDetail"];
            try
            {
                var validateLogin = new CustomMemberShip();
                if (currentUser != null)
                {   
                    flag = validateLogin.ChangePassword(currentUser.UserName, passwordModel.OldPassword, passwordModel.NewPassword.ToMD5HashForPassword());
                    if (flag)
                        Session["Tempmsg"] = "[[[Password has been changed successfully]]]";
                    userName = currentUser.UserName;
                    regID = currentUser.RegistrationId;
                }
            }
            catch (Exception ex)
            {
                logService.LogError("Error in UpdatePassword", ex);
            }

            //if ((string)Session["RoleName"] == NoidaAuthority.PMS.Common.Contants.Roles.Customer.ToString())
            //{
            //    return RedirectToAction("Citizen", "Customer");
            //    //return RedirectToAction("Index", "Customer", new { id = regID });
            //}
            //else if ((string)Session["RoleName"] == NoidaAuthority.PMS.Common.Contants.Roles.Administrator.ToString())
            //{
            //    return RedirectToAction("Dashboard", "Home");
            //}
            //else if ((string)Session["RoleName"] == NoidaAuthority.PMS.Common.Contants.deptHead)
            //{
            //    return RedirectToAction("Dashboard", "Home");
            //}
            //else
            //    return RedirectToAction("ManageUsers", "ManageUsers");

            if (currentUser.RoleId == 0)
            {
                return Redirect("/Administration/Admin/ManageEmployee");
            }
            else if (currentUser.RoleId == 1)
            {
                //return Redirect("/Administration/Property/Dashboard");
                return RedirectToAction("Dashboard", "Property", new { area = "Administration" });
            }
            else
            {
                //return Redirect("/Account/ChangePassword");
                return Redirect("/Customer/Property/Dashboard");
            }
        }

        [AllowAnonymous]
        public JsonResult CheckOldPassword(String OldPassword)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                if (Session["UserDetail"] != null)
                {
                    var currentUser = (UserViewModel)Session["UserDetail"];
                    //var userRecord = validateLogin.GetUserDetails(currentUser.UserName);
                    var userRecord = validateLogin.GetUserDetailsByUserName(currentUser.UserName);
                    if (userRecord.Password == OldPassword.ToMD5HashForPassword())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logService.LogError("Error in CheckOldPassword", ex);
            }
            return Json(flag);
        }
        [AllowAnonymous]
        public JsonResult CheckEmailAddress(String Email)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = validateLogin.CheckEmailAddressExists(Email);
            }
            catch (Exception ex)
            {
                logService.LogError("Error in CheckEmailAddress", ex);
            }
            return Json(flag);
        }
        [AllowAnonymous]
        public JsonResult CheckEmailAddressRegistation(String Email)
        {
            var flag = false;
            try
            {
                var validateLogin = new CustomMemberShip();
                flag = !validateLogin.CheckEmailAddressExists(Email);
            }
            catch (Exception ex)
            {
                logService.LogError("Error in CheckEmailAddressRegistation", ex);
            }
            return Json(flag);
        }

        //added by Shatrughna kumar
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
        public JsonResult CheckRegistationID(String PropertyId)
        {
            var flag = false;
            try
            {
                PropertyService _propertyService = new PropertyService();
                DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
                if (!string.IsNullOrEmpty(PropertyId))
                {
                    int _PropertyId = 0;
                    if (int.TryParse(PropertyId, out _PropertyId))
                        PropertyFilterObj.RegistrationId = _PropertyId;
                }
                IEnumerable<DtoProperty> propertyList = _propertyService.GetPropertyList(PropertyFilterObj);
                if (propertyList.Count() > 0)
                    flag = true;

            }
            catch (Exception ex)
            {
                logService.LogError("Error in CheckRegistationID", ex);
            }
            return Json(flag);
        }
        [AllowAnonymous]
        public JsonResult CheckEmailAddressExistsOrLocked(String Email)
        {
            var validateLogin = new CustomMemberShip();
            var flag = validateLogin.CheckEmailAddressExists(Email);

            if (flag)
            {
                return Json("Username not exists.", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var userRecord = validateLogin.GetUserDetails(Email);
                if (userRecord.IsLockedOut)
                {
                    return Json("Username not exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        ModelState.Clear();
        //        Session["allMenudIdsForRoleUser"] = null;
        //        //var flag = Membership.ValidateUser(model.Email, model.Password.ToMD5HashForPassword());
        //        var flag = true;
        //        if (flag)
        //        {
        //            Session["AttemptCount"] = null;
        //            var customMemberShip = new CustomMemberShip();
        //            //var userRecord = customMemberShip.GetUserDetails(model.Email);

        //            //if (userRecord.Status != null && (!userRecord.IsLockedOut && userRecord.Status.Value))
        //            //{
        //                var user = new CurrentUserDetail();
        //                //user.UserId = userRecord.UserId;
        //                user.FirstName = "Shatrughna";// userRecord.FirstName;
        //                user.LastName = "kumar"; // userRecord.LastName;
        //                user.Email = "skumar@svam.com";
        //                user.RegistrationId = "10000013";
        //                user.MobileNo = "8527036431";
        //                user.UserEmail = "skumar@svam.com";

        //                user.FullName = "Shatrughna kumar";

        //                var serializer = new JavaScriptSerializer();
        //                string userData = serializer.Serialize(user);

        //                Session["CurrentUser"] = user;
        //                Session["UserName"] = user.FirstName;
        //                Session["RegistrationID"] = user.RegistrationId;
        //                //Session["Roles"] = _manageRoleService.GetRoleForUser(model.Email);
        //                //var roles = _manageRoleService.GetRoleForUser(model.Email);
        //                Session["RoleID"] = "2";
        //                Session["RoleName"] = "Customer";
        //                Session["DeptId"] = 1;
        //                //if (userRecord.DeptId != 0)
        //                //{
        //                //    ICommonService commonService;
        //                //    commonService = new CommonService();
        //                //    IList<DtoList> list = null;
        //                //    list = commonService.GetPropertyType();
        //                //    foreach (DtoList lst in list)
        //                //    {
        //                //        if (lst.value == userRecord.DeptId.ToString())
        //                //            Session["DeptName"] = lst.label;
        //                //    }
        //                //}
        //                //  var RoleId = Session["RoleId"];
        //                var authTicket = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), model.RememberMe, userData, FormsAuthentication.FormsCookiePath);

        //                string encTicket = FormsAuthentication.Encrypt(authTicket);
        //                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //                Response.Cookies.Add(faCookie);
        //                //await SignInAsync(user, model.RememberMe);
        //                //var customUrl = "/Customer/Index/" + currentUserDetail.RegistrationId;
        //                var customUrl = "/Customer/Property/Dashboard";

        //                if (Session["RoleID"].Equals("1") || Session["RoleID"].Equals("4") || Session["RoleID"].Equals("5"))
        //                {
        //                    return RedirectToLocal(returnUrl);
        //                }
        //                else
        //                {
        //                    // RedirectToLocal(customUrl);
        //                    return Redirect(customUrl);
        //                    // Response.Redirect(customUrl);
        //                }



        //            //}
                    
        //        }
        //        else
        //        {

        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        
        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        ModelState.Clear();
        //        Session["allMenudIdsForRoleUser"] = null;
        //        var flag = Membership.ValidateUser(model.Email, model.Password.ToMD5HashForPassword());
        //        //flag = true;
        //        if (flag)
        //        {
        //            Session["AttemptCount"] = null;
        //            var customMemberShip = new CustomMemberShip();
        //            var userRecord = customMemberShip.GetUserDetails(model.Email);

        //            if (userRecord.Status != null && (!userRecord.IsLockedOut && userRecord.Status.Value))
        //            {
        //                var currentUserDetail = new CurrentUserDetail();
        //                currentUserDetail.UserId = userRecord.UserId;
        //                currentUserDetail.FirstName = userRecord.FirstName;
        //                currentUserDetail.LastName = userRecord.LastName;
        //                currentUserDetail.Email = model.Email;
        //                currentUserDetail.RegistrationId = userRecord.PropertyId;
        //                currentUserDetail.MobileNo = userRecord.MobileNo;
        //                currentUserDetail.UserEmail = userRecord.UserEmail;

        //                currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);

        //                var serializer = new JavaScriptSerializer();
        //                string userData = serializer.Serialize(currentUserDetail);

        //                Session["CurrentUser"] = currentUserDetail;
        //                Session["UserName"] = currentUserDetail.FirstName;
        //                Session["RegistrationID"] = currentUserDetail.RegistrationId;
        //                Session["Roles"] = _manageRoleService.GetRoleForUser(model.Email);
        //                var roles = _manageRoleService.GetRoleForUser(model.Email);
        //                Session["RoleID"] = _manageRoleService.GetRoleForUser(model.Email).RoleId.ToString();
        //                Session["RoleName"] = roles.RoleName;
        //                Session["DeptId"] = userRecord.DeptId;
        //                if (userRecord.DeptId != 0)
        //                {
        //                    ICommonService commonService;
        //                    commonService = new CommonService();
        //                    IList<DtoList> list = null;
        //                    list = commonService.GetPropertyType();
        //                    foreach (DtoList lst in list)
        //                    {
        //                        if (lst.value == userRecord.DeptId.ToString())
        //                            Session["DeptName"] = lst.label;
        //                    }
        //                }
        //                //  var RoleId = Session["RoleId"];
        //                var authTicket = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), model.RememberMe, userData, FormsAuthentication.FormsCookiePath);

        //                string encTicket = FormsAuthentication.Encrypt(authTicket);
        //                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //                Response.Cookies.Add(faCookie);
        //                //await SignInAsync(user, model.RememberMe);
        //                //var customUrl = "/Customer/Index/" + currentUserDetail.RegistrationId;

        //                //var customUrl = "/Customer/Citizen";
        //                var customUrl = "/Customer/Property/Dashboard";

        //                if (Session["RoleID"].Equals("1") || Session["RoleID"].Equals("4") || Session["RoleID"].Equals("5"))
        //                {
        //                    return RedirectToLocal(returnUrl);
        //                }
        //                else
        //                {
        //                    // RedirectToLocal(customUrl);
        //                    return Redirect(customUrl);
        //                    // Response.Redirect(customUrl);
        //                }
        //            }
        //            else
        //            {
        //                if (userRecord.IsLockedOut)
        //                    ModelState.AddModelError("ErrorMessage", "Your Account has been locked, Please Contact to Administration.");
        //                if (userRecord.Status != null && !userRecord.Status.Value)
        //                    ModelState.AddModelError("ErrorMessage", "User is inactive.");
        //            }
        //        }
        //        else
        //        {

        //            if (Session["AttemptCount"] == null)
        //            {
        //                Session["AttemptCount"] = 1;
        //            }
        //            else
        //            {
        //                //ModelState.Clear();
        //                var attempts = (Int32)Session["AttemptCount"];

        //                if (attempts == 5)
        //                {
        //                    attempts = 5;
        //                }
        //                else
        //                {
        //                    ++attempts;
        //                }
        //                if (attempts == 4)
        //                {
        //                    ModelState.AddModelError("ErrorMessage", "This is your last attempt to enter the password.");
        //                }
        //                if (attempts == 5)
        //                {
        //                    attempts = 5;
        //                    var customMemberShip = new CustomMemberShip();
        //                    customMemberShip.LockUser(model.Email);
        //                    ModelState.AddModelError("ErrorMessage", "Your Account has been locked, Please Contact to Administration.");
        //                }
        //                Session["AttemptCount"] = attempts;

        //            }
        //            ModelState.AddModelError("ErrorMessage", "Invalid username or password.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        public ActionResult Logout()
        {
            Session["allMenudIdsForRoleUser"] = null;
            Session["CurrentUser"] = null;
            Session["UserDetail"] = null;
            Session["UserName"] = null;
            Session["RoleName"] = null;
            FormsAuthentication.SignOut();
            Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var flag = false;
            var userFullName = String.Empty;
            try
            {
                if (ModelState.IsValid)
                {

                    var passwordText = CommonMethods.GetRandomPassword();
                    var validateLogin = new CustomMemberShip();
                    flag = validateLogin.ChangePassword(model.Email, "", passwordText);

                    //if (userRecord.LastPasswordChangeDate.HasValue)
                    //    {
                    //        //Sample for Sending Email
                    //        //string userFullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                    //        //var templateVars = new Hashtable();
                    //        //templateVars.Add("LoginURL", "test");
                    //        //templateVars.Add("Password", "test");

                    //        //templateVars.Add("UserFullName", userFullName);
                    //        //var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ForgotPassword.html"), templateVars);
                    //        //var emailBody = parser.Parse(); 
                    //        //EmailHelper emailHelper = new EmailHelper();
                    //        //emailHelper.Send("sudhirmangla@gmail.com", "test", emailBody);

                    //        return RedirectToLocal(returnUrl);
                    //    }
                    //    else
                    //        return RedirectToAction("ChangePassword");
                    //}




                    if (flag)
                    {
                        var userRecord = validateLogin.GetUserDetails(model.Email);
                        userFullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                        var emailLink = ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString();
                        var templateVars = new Hashtable();
                        templateVars.Add("LoginURL", emailLink);
                        templateVars.Add("Password", passwordText);
                        //Updated changes for bug #6128
                        templateVars.Add("UserFullName", userFullName);
                        var parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/ForgotPassword.html"), templateVars);
                        var emailBody = parser.Parse();
                        EmailHelper emailHelper = new EmailHelper();
                        emailHelper.Send(model.Email, "Test_Mail", emailBody);


                    }
                    // return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }
            catch (Exception)
            {
                throw;
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult MyProfile(NaUser model)
        {
            //_userService = new ManageUserService();
            if (Session["CurrentUser"] != null)
            {
                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                model.UserId = currentUser.UserId;

                model = _userService.GetUserById(model.UserId);
            }
            return View(model);
        }

        public ActionResult MyProfileUpdate(NaUser model)
        {
            var result = false;
            //_userService = new ManageUserService();
            if (Session["CurrentUser"] != null)
            {

                var currentUser = (CurrentUserDetail)Session["CurrentUser"];
                model.CreatedBy = currentUser.UserId;
                model.UserId = currentUser.UserId;
                model.Status = Contants.status;
                result = _userService.AddEditUser(model);//_userService.GetUserById(model.UserId);

            }

            //ViewBag.Title = "Edit Customer";
            ////NaUser model = new NaUser();
            //if (uid != new Guid())
            //{
            //    Session["UserUid"] = uid;
            //    model = manageUserService.GetUserById(uid);
            //    ViewBag.isUpdateMode = true;

            //}


            ViewBag.message = "Your data has been updated";
            return View("MyProfile", model);//RedirectToAction("Index", "Home");
        }

        public ActionResult Reset(LoginViewModel user)
        {
            user.Email = "";
            user.Password = "";

            return View(user);
        }

        [AllowAnonymous]
        public JsonResult ResetPassword(string emailID)
        {
            //_userService = new ManageUserService();
            var rslt = _userService.ResetPassword(emailID);
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new UserViewModel();
            model.RandomKeyValue = RandomString(7);
            model.Captcha = RandomString(7);
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

        [AllowAnonymous]
        [HttpPost]
        public JsonResult RegisterCustomer(UserViewModel model, HttpPostedFileBase fileI, HttpPostedFileBase fileII)
        {
            int flag = _userService.SaveNACustomerDetails(model, fileI, fileII);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetSecurityQuestionList()
        {
            var list = _commonService.GetSecurityQuestionList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetMiscellaneousTypeList(DropdownViewModel model)
        {
            var list = _commonService.GetMiscellaneousTypeList(model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

         [AllowAnonymous]
        public JsonResult ValidateRegistrationId(PropertyViewModel model)
        {
            PropertyViewModel data = _userService.ValidateRegistrationId(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult SendOTPOnMobileAndEmail(ActionViewModel model)
        {
            ActionViewModel result = _commonService.SendMessageOnMobileAndEmail(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetSectorList(DropdownViewModel model)
        {
            var sectors = _commonService.GetSectorList(model);
            return Json(sectors, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetBlockList(DropdownViewModel model)
        {
            var sectors = _commonService.GetBlockList(model);
            return Json(sectors, JsonRequestBehavior.AllowGet);
        }
    }
}