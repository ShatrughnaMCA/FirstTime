using System.Web.Mvc;

namespace NoidaAuthority.PMS.Web.Controllers.Common
{
    public class CommonController : Controller
    {
        //page not found
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        [ChildActionOnly]
        public ActionResult JavaScriptDisabledWarning()
        {
            return PartialView();
        }


    }
}