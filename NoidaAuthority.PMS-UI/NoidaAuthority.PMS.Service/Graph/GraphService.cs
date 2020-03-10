using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;
using System.Collections.Generic;
namespace NoidaAuthority.PMS.Service
{
    public class GraphService :IGraphService
    {
        IGraphRepository graphRepository;
        public GraphService()
        {
            graphRepository = new GraphRepository();
        }

        public IEnumerable<DtoPropertyType> GetGraphDataByPropertyType(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetPropertyTypeGraph(objPropertyFilter);
        }

        public IEnumerable<DtoAllotmentStatus> GetGraphDataByPropertyStatus(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetAllotmentStatusGraph(objPropertyFilter);
        }

        public IEnumerable<DtoAreaDetails> GetGraphDataByPropertyArea(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetAreaGraph(objPropertyFilter);
        }

        public IEnumerable<DtoYearwiseRevenue> GetGraphDataByPropertyRevenue(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetRevenueDetailsGraph(objPropertyFilter);
        }

        public IEnumerable<DtoPropertyType> GetGraphDataByPropertyRevenue1(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetRevenueDetailsGraph1(objPropertyFilter);
        }
        public IEnumerable<DtoPropertyAreaType> GetGraphDataByAreaWisePropertyType(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetAreaWisePropertyTypeGraph(objPropertyFilter);
        }

        public IEnumerable<DtoRevenueDefaultedGraph> GetRevenueDefaultedGraph(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetRevenueDefaultedGraph(objPropertyFilter);
        }
        public IEnumerable<DtoDepartmentwiseRevenueGraph> GetDepartmentWiseRevenueDefaulted(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetDepartmentWiseRevenueDefaulted(objPropertyFilter);
        }
        public IEnumerable<DtoRevenueSource> GetRevenueSourceTypes(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetRevenueHeadwiseGraph(objPropertyFilter);
        }
        public IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetPropertyComplaintGraph(objPropertyFilter);
        }
        public IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph1(DtoPropertyFilter objPropertyFilter)
        {
            return graphRepository.GetPropertyComplaintGraph1(objPropertyFilter);
        }
    }
}
