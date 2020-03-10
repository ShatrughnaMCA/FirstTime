using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Service
{
   public interface IGraphService
    {
       IEnumerable<DtoPropertyType> GetGraphDataByPropertyType(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoAllotmentStatus> GetGraphDataByPropertyStatus(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoAreaDetails> GetGraphDataByPropertyArea(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoYearwiseRevenue> GetGraphDataByPropertyRevenue(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoPropertyAreaType> GetGraphDataByAreaWisePropertyType(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoRevenueDefaultedGraph> GetRevenueDefaultedGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoDepartmentwiseRevenueGraph> GetDepartmentWiseRevenueDefaulted(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoRevenueSource> GetRevenueSourceTypes(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph1(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoPropertyType> GetGraphDataByPropertyRevenue1(DtoPropertyFilter objPropertyFilter);
    }
}
