using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Common.Logger;
using NoidaAuthority.PMS.Web.Helper;
using StackExchange.Profiling;

namespace NoidaAuthority.PMS.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IUnityContainer container = Bootstrapper.Initialise();

            ViewEngines.Engines.Clear();
            IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(razorEngine);
            //Enabling Bundling and Minification
            BundleTable.EnableOptimizations = false; 
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            //log error
            LogException(exception);

        }
        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;


            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

            try
            {
                //log
                var logger = LoggingManager.GetLogInstance();
                logger.LogError("Global Exception", exc);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }

        protected void Application_BeginRequest()
        {
            if (SettingsHelper.IsMiniProfilerEnabled)
            {
                //if (Request.IsLocal)
                {
                    MiniProfiler.Start();
                }
            }
        }

        protected void Application_EndRequest()
        {
            if ( SettingsHelper.IsMiniProfilerEnabled)
            {
                MiniProfiler.Stop();
            }
        }

        private void Session_Start(object sender, EventArgs e)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;

            //log.InfoFormat("Starting session: {0} from {1}.", Session.SessionID, ip);

            if ((HttpContext.Current != null) &&
                (HttpContext.Current.User != null) &&
                (HttpContext.Current.User.Identity.IsAuthenticated))
            {
                var loginHelper = new LoginHelper();
                loginHelper.LoginHelperInitialize();
            }
        }
    }
}