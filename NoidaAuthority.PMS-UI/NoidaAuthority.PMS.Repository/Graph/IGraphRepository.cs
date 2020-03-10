using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoidaAuthority.PMS.Entity;
namespace NoidaAuthority.PMS.Repository
{
   public interface IGraphRepository
    {
       IEnumerable<DtoPropertyType> GetPropertyTypeGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoAllotmentStatus> GetAllotmentStatusGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoAreaDetails> GetAreaGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoYearwiseRevenue> GetRevenueDetailsGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoPropertyAreaType> GetAreaWisePropertyTypeGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoRevenueDefaultedGraph> GetRevenueDefaultedGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoDepartmentwiseRevenueGraph> GetDepartmentWiseRevenueDefaulted(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoRevenueSource> GetRevenueHeadwiseGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph1(DtoPropertyFilter objPropertyFilter);
       IEnumerable<DtoPropertyType> GetRevenueDetailsGraph1(DtoPropertyFilter objPropertyFilter);
       
        

    }
}
