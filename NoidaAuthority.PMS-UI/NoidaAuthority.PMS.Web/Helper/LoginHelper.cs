using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using NoidaAuthority.PMS.Service.Admin;
using NoidaAuthority.PMS.Web.Controllers.Common;

namespace NoidaAuthority.PMS.Web.Helper
{
    public class LoginHelper
    {
        public bool LoginHelperInitialize()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (authCookie == null)
                return false;
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var customMemberShip = new CustomMemberShip();
            if (authTicket != null)
            {
                var userRecord = customMemberShip.GetUserDetails(authTicket.Name);
                if (userRecord.Status != null && userRecord.RoleId.Equals("1") && (!userRecord.IsLockedOut && userRecord.Status.Value))
                {
                    var currentUserDetail = new CurrentUserDetail();
                    currentUserDetail.UserId = userRecord.UserId;
                    currentUserDetail.FirstName = userRecord.FirstName;
                    currentUserDetail.LastName = userRecord.LastName;
                    currentUserDetail.RegistrationId = userRecord.PropertyId;
                    currentUserDetail.Email = authTicket.Name;
                    currentUserDetail.FullName = String.Format("{0} {1}", userRecord.FirstName, userRecord.LastName);
                    var serializer = new JavaScriptSerializer();
                    string userData = serializer.Serialize(currentUserDetail);
                    IManageRoleService _manageRoleService = new ManageRoleService();
                    HttpContext.Current.Session["CurrentUser"] = currentUserDetail;
                    HttpContext.Current.Session["RegistrationID"] = currentUserDetail.RegistrationId;
                    HttpContext.Current.Session["UserName"] = currentUserDetail.FirstName;
                    HttpContext.Current.Session["Roles"] = _manageRoleService.GetRoleForUser(authTicket.Name);
                    HttpContext.Current.Session["RoleID"] = _manageRoleService.GetRoleForUser(authTicket.Name).RoleId.ToString();
                    return true;
                }
                else
                {
                    SignOut();
                    //redirect to login page
                    var virtualPathData =
                        RouteTable.Routes.GetVirtualPath(((MvcHandler)HttpContext.Current.CurrentHandler).RequestContext,
                            new RouteValueDictionary(
                                new { controller = "Account", action = "Login", id = "" }));
                    if (virtualPathData != null)
                    {
                        string url = virtualPathData.VirtualPath;
                        HttpContext.Current.Response.Redirect(url);
                        return false;
                    }
                }
            }
            return false;

        }
        public void SignOut()
        {
            //HttpContext.Current.Session["allMenudIdsForRoleUser"] = null;
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);
        }
    }
}