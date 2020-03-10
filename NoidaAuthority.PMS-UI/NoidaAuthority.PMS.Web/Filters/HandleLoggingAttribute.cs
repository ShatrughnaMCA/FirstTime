using System.Web.Mvc;
using NoidaAuthority.PMS.Common.Extensions;
using NoidaAuthority.PMS.Common.Logger;
using System;
using System.Web;

namespace NoidaAuthority.PMS.Web.Filters
{
    public class HandleLoggingAttribute : ActionFilterAttribute
    {
        protected ILogService Logger = LoggingManager.GetLogInstance();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Logger.IsDebugEnabled)
            {
                var actionDescriptor = filterContext.ActionDescriptor;
                string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = actionDescriptor.ActionName;
                string userName = filterContext.HttpContext.User.Identity.Name;

                var message = "Executing Action: {0} on Controller: {1} For User : {2}.".FormatWith(actionName, controllerName, userName);
                if (filterContext.RouteData.Values["id"] != null)
                {
                    string routeId = filterContext.RouteData.Values["id"].ToString();
                    message = "Executing Action: {0} on Controller: {1}  For User : {2} Given Route: {3}.".FormatWith( actionName, controllerName, userName, routeId);
                }

                Logger.LogDebug(message);
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Logger.IsDebugEnabled)
            {
                var actionDescriptor = filterContext.ActionDescriptor;
                string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = actionDescriptor.ActionName;
                string userName = filterContext.HttpContext.User.Identity.Name;

                var message = "Executed Action: {0} on Controller: {1} For User : {2}.".FormatWith(actionName, controllerName, userName);
                if (filterContext.RouteData.Values["id"] != null)
                {
                    string routeId = filterContext.RouteData.Values["id"].ToString();
                    message = "Executed Action: {0} on Controller: {1} For User : {2} Given Route: {3}.".FormatWith( actionName, controllerName, userName, routeId);
                }

                Logger.LogDebug(message);
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            base.OnResultExecuting(filterContext);
        }
    }
}