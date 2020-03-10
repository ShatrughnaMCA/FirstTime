using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Web.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NoidaAuthority.PMS.Web.Controllers
{
    public class GeneralApiController : ApiController
    {
        ICommonService commonService;
        DtoPropertyFilter objPropertyFilter;
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
        public IList<DtoList> GetList()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string name = nvc["name"];
            string id = nvc["id"];
            commonService = new CommonService();
            objPropertyFilter = new DtoPropertyFilter();
            IList<DtoList> list = null;
            switch (name)
            {
                case Contants.QueryString.PropertyType:
                    list = commonService.GetPropertyType();
                    break;
                case Contants.QueryString.PropertyStatus:
                    list = commonService.GetAlotmentStatus();
                    break;
                case Contants.QueryString.Sector:
                    list = commonService.GetSector(Convert.ToInt32(id));
                    break;
                case Contants.QueryString.Block:
                    objPropertyFilter.Sector = id;
                    list = commonService.GetBlock(objPropertyFilter);
                    break;
                case Contants.QueryString.PropertyNumber:
                    objPropertyFilter = PropertyHelper.CreatePropertyFilter(Request);
                    list = commonService.GetPropertyNo(objPropertyFilter);
                    break;
                case Contants.QueryString.AllotteeName:
                    objPropertyFilter = PropertyHelper.CreatePropertyFilter(Request);
                    list = commonService.GetAlloteName(objPropertyFilter);
                    break;
                case Contants.QueryString.PlotNumber:
                    objPropertyFilter = PropertyHelper.CreatePropertyFilter(Request);
                    list = commonService.GetPlotNumber(objPropertyFilter);
                    break;
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
