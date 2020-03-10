using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoidaAuthority.PMS.Web.Filters
{
    public class NAAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            UserViewModel user = null;
            bool unAuthorize = false;
            if (HttpContext.Current.Session["UserDetail"] != null)
            {
                user = (UserViewModel)(HttpContext.Current.Session["UserDetail"]);
                if (user.RoleId != NAConstant.SuperAdminRoleId && user.RoleId != NAConstant.AdminRoleId && user.RoleId != NAConstant.CustomerRoleId)
                {
                    unAuthorize = true;
                }

                if (unAuthorize)
                {
                    filterContext.Result = filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
            else
            {
                filterContext.Result = filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }


            HttpRequestBase request = filterContext.HttpContext.Request;
            

            //if (filterContext == null || filterContext.HttpContext == null) return;

            //HttpRequestBase request = filterContext.HttpContext.Request;

            //if (request == null) return;

            ////don't apply filter to child methods
            //if (filterContext.IsChildAction) return;

            //if (!string.IsNullOrEmpty(filterContext.RouteData.Values["controller"].ToString()) && !string.IsNullOrEmpty(filterContext.RouteData.Values["action"].ToString())) { }
            //else return;
            

            //UserViewModel user = null;
            //if (filterContext.HttpContext.Request.IsAuthenticated)
            //{
            //    if (HttpContext.Current.Session["UserDetail"] != null)
            //    {
            //        user = (UserViewModel)(HttpContext.Current.Session["UserDetail"]);
            //    }

                
            //    if (user != null)
            //    {
            //        if (user.RoleId != NAConstant.SuperAdminRoleId && user.RoleId != NAConstant.AdminRoleId && user.RoleId != NAConstant.CustomerRoleId)
            //        {
            //            unAuthorize = true;
            //        }

            //        if (unAuthorize)
            //        {
            //            filterContext.Result = filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            //        }
            //    }
            //    else
            //    {
            //        filterContext.Result = filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            //    }
            //}
        }
    }
}