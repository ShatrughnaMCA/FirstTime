using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Service.Admin;

namespace NoidaAuthority.PMS.Web.Filters
{
    public class ValidateUserRoleMenus : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext == null || filterContext.HttpContext == null)
                return;
            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            if (filterContext.RouteData.Values["controller"] != null &&
                filterContext.RouteData.Values["action"] != null &&
                filterContext.RouteData.Values["controller"].ToString().Trim().Length > 0 &&
                filterContext.RouteData.Values["action"].ToString().Trim().Length > 0)
            {
            }
            else
            {
                return;
            }

            Role role = null;

            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.Current.Session["Roles"] != null)
                {
                    role = (Role)(HttpContext.Current.Session["Roles"]);
                }

                bool unAuthorize = false;
                if (role != null)
                {
                    if (role.RoleId != Contants.AdminRoleId && role.RoleId != Contants.DeptRoleId && role.RoleId != Contants.BackOfficeRoleId)
                    {
                        unAuthorize = true;
                    }


                    if (unAuthorize)
                    {
                        filterContext.Result = filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login" }));
                        ;
                    }
                }
                else
                {
                    filterContext.Result = filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    ;
                }
            }
        }
    }
}