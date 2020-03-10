using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NoidaAuthority.PMS.Web.Areas.Administration.Controllers
{
    public class DefaultApiController : ApiController
    {
        ICommonService commonService;
        PropertyFilterViewModel propertyFilter;
        // GET api/common
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/common/5
        public string Get(int id)
        {
            return "value";
        }

        [ActionName("List")]
        public IList<DropdownViewModel> GetList()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string name = nvc["name"];
            string id = nvc["id"];
            commonService = new CommonService();
            propertyFilter = new PropertyFilterViewModel();
            IList<DropdownViewModel> list = null;
            switch (name)
            {
                //case Contants.QueryString.PropertyType:
                //    list = commonService.GetPropertyType();
                //    break;
                //case Contants.QueryString.PropertyStatus:
                //    list = commonService.GetAlotmentStatus();
                //    break;
                case NAQueryString.Sector:
                    //list = commonService.GetSector(Convert.ToInt32(id));
                    list = commonService.GetSectorList(new DropdownViewModel());
                    break;
                //case Contants.QueryString.Block:
                //    propertyFilter.Sector = id;
                //    list = commonService.GetBlock(propertyFilter);
                //    break;
                //case Contants.QueryString.PropertyNumber:
                //    propertyFilter = PropertyHelper.CreatePropertyFilter(Request);
                //    list = commonService.GetPropertyNo(propertyFilter);
                //    break;
                //case Contants.QueryString.AllotteeName:
                //    propertyFilter = PropertyHelper.CreatePropertyFilter(Request);
                //    list = commonService.GetAlloteName(propertyFilter);
                //    break;
                //case Contants.QueryString.PlotNumber:
                //    propertyFilter = PropertyHelper.CreatePropertyFilter(Request);
                //    list = commonService.GetPlotNumber(propertyFilter);
                //    break;
            }
            return list;
        }

        public List<DtoList> GetLegalDDL()
        {
            commonService = new CommonService();
            var list = commonService.GetLegalDDL();
            return list;
        }

        public List<DtoList> GetRevYears()
        {
            commonService = new CommonService();
            var list = commonService.GetRevYears();
            return list;
        }

        // POST api/common
        public void Post([FromBody]string value)
        {
        }

        // PUT api/common/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/common/5
        public void Delete(int id)
        {
        }

    }
}
