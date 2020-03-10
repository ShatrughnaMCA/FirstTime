using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoidaAuthority.PMS.Web.Controllers.Common;
using NoidaAuthority.PMS.Web.Filters;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;

namespace NoidaAuthority.PMS.Web.Controllers
{
    public class HomeController : WebBaseController
    {
        //
        // GET: /Home/
        [ValidateUserRoleMenus]
        public ActionResult Index()
        {
            if (Session["RoleId"].ToString().Trim() == "5")
            {
                //return RedirectToAction("ManageUsers", "ManageUsers");
                return RedirectToAction("ManageCustomer", "Admin", new { area = "Administration" });
            }
            else
            {                
                //return RedirectToAction("Dashboard", "Home");
                return RedirectToAction("Dashboard", "Admin", new { area="Administration"});
            }
        }
        [ValidateUserRoleMenus]
        public ActionResult Dashboard()
        {
            CommonService cms = new CommonService();
            var pCount = cms.GetPropertyCount();
            ViewData["PropCount"] = pCount;
            return View();
        }


        public ActionResult RegisterApplicant()
        {
            return View();
        }

	}
}