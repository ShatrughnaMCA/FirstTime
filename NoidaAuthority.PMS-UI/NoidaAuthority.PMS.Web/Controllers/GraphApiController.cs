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
    public class GraphApiController : ApiController
    {
        IGraphService _graphService;
        // GET api/Graphp
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IEnumerable<string> Get(int id)
        {
            return new string[] { "value1", "value2" };
        }

        [ActionName("PropertyType")]
        public IEnumerable<DtoPropertyType> GetByPropertyType()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoPropertyType> resultList = _graphService.GetGraphDataByPropertyType(objPropertyFilter);
            return resultList;
        }
        [ActionName("PropertyArea")]
        public IEnumerable<DtoAreaDetails> GetByPropertyArea()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoAreaDetails> resultList = _graphService.GetGraphDataByPropertyArea(objPropertyFilter);
            return resultList;
        }
        [ActionName("PropertyStatus")]
        public IEnumerable<DtoAllotmentStatus> GetByPropertyStatus()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoAllotmentStatus> resultList = _graphService.GetGraphDataByPropertyStatus(objPropertyFilter);
            return resultList;
        }
        [ActionName("PropertyRevenue")]
        public IEnumerable<DtoYearwiseRevenue> GetByPropertyRevenue()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoYearwiseRevenue> resultList = _graphService.GetGraphDataByPropertyRevenue(objPropertyFilter);
            return resultList;
        }


        [ActionName("PropertyRevenue1")]
        public IEnumerable<DtoPropertyType> GetByPropertyRevenue1()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoPropertyType> resultList = _graphService.GetGraphDataByPropertyRevenue1(objPropertyFilter);
            return resultList;
        }

        [ActionName("RevenueSourceType")]
        public IEnumerable<DtoRevenueSource> GetRevenueSourceType()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = PropertyHelper.CreatePropertyFilter(Request);
            IEnumerable<DtoRevenueSource> resultList = _graphService.GetRevenueSourceTypes(objPropertyFilter);
            return resultList;
        }
        [ActionName("AreaPropertyType")]
        public IEnumerable<DtoPropertyAreaType> GetByAreaPropertyType()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoPropertyAreaType> resultList = _graphService.GetGraphDataByAreaWisePropertyType(objPropertyFilter);
            return resultList;
        }

        // Property by Payment
        [ActionName("RevenueDefaulted")]
        public IEnumerable<DtoRevenueDefaultedGraph> GetByRevenueDefaulted()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoRevenueDefaultedGraph> resultList = _graphService.GetRevenueDefaultedGraph(objPropertyFilter);
            return resultList;
        }

        // Property by DepartmentwiseRevenue
        [ActionName("DepartmentWiseRevenue")]
        public IEnumerable<DtoDepartmentwiseRevenueGraph> GetByDepartmentwiseRevenue()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoDepartmentwiseRevenueGraph> resultList = _graphService.GetDepartmentWiseRevenueDefaulted(objPropertyFilter);
            return resultList;
        }
        // Property by DepartmentwiseRevenue
        [ActionName("PropertyComplaintGraph")]
        public IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoComplaintGraph> resultList = _graphService.GetPropertyComplaintGraph(objPropertyFilter);
            return resultList;
        }


        [ActionName("PropertyComplaintGraph1")]
        public IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph1()
        {
            _graphService = new GraphService();
            DtoPropertyFilter objPropertyFilter = CreatePropertyFilter();
            IEnumerable<DtoComplaintGraph> resultList = _graphService.GetPropertyComplaintGraph1(objPropertyFilter);
            return resultList;
        }


        private DtoPropertyFilter CreatePropertyFilter()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            DtoPropertyFilter objPropertyFilter = new DtoPropertyFilter();
            string sector = nvc[Contants.QueryString.Sector];
            string deptId = nvc[Contants.QueryString.DeptId];
            if (!string.IsNullOrEmpty(sector))
                objPropertyFilter.Sector = sector;
            if (!string.IsNullOrEmpty(deptId))
                objPropertyFilter.PropertyTypeId = deptId;
            return objPropertyFilter;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
