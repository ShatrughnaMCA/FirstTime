using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Web.Filters;

namespace NoidaAuthority.PMS.Web.Controllers.Common
{
    [HandleLogging]
    [NAAuthorize]
    public abstract class WebBaseController : Controller
    {
        //
        // GET: /BasePricePoint/
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            IController errorController = new CommonController();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(HttpContext, routeData));

            return new EmptyResult();
        }

        /// <summary>
        ///     On exception
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
                LogException(filterContext.Exception);
            base.OnException(filterContext);
        }

        /// <summary>
        ///     Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        private void LogException(Exception exc)
        {
            var logger = LoggingManager.GetLogInstance();
            logger.LogError("Controller Exception", exc);
        }

        public string GetNameChanged(string inString) //UAT bug fixes
        {
            var outString = string.Empty;
            if (!string.IsNullOrEmpty(inString))
            {
                outString = Regex.Replace(inString, @"[^0-9a-zA-Z]+", string.Empty);
                //outString = inString.Replace(" ", string.Empty);
                //outString = outString.Replace("/", string.Empty);
            }
            return outString;
        }
    }
}