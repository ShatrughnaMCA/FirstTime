using Dapper;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
//using NoidaAuthority.PMS.Repository;

namespace NoidaAuthority.PMS.Repository
{

    public class GraphRepository : IGraphRepository
    {
        const string storedProcedure = "dbo.sp_GetPropertyGraph";
        #region Graph Method
        public IEnumerable<DtoPropertyType> GetPropertyTypeGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoPropertyType>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyType }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoAllotmentStatus> GetAllotmentStatusGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoAllotmentStatus>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyStatus }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoAreaDetails> GetAreaGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoAreaDetails>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyArea }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoYearwiseRevenue> GetRevenueDetailsGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoYearwiseRevenue>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyRevenue }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoPropertyType> GetRevenueDetailsGraph1(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                //return connection.Query<DtoPropertyType>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyType }, commandType: CommandType.StoredProcedure).ToList();
                return connection.Query<DtoPropertyType>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyRTIGraph }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoPropertyAreaType> GetAreaWisePropertyTypeGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoPropertyAreaType>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyTypeArea }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoRevenueDefaultedGraph> GetRevenueDefaultedGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoRevenueDefaultedGraph>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyRevenueTrendGraph }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoDepartmentwiseRevenueGraph> GetDepartmentWiseRevenueDefaulted(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoDepartmentwiseRevenueGraph>("dbo.sp_GetDepartmentWiseRevenueDefaultedGraph", new { SectorId = objPropertyFilter.Sector, monYear = objPropertyFilter.MonYear, ProperyTypeId = objPropertyFilter.PropertyTypeId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoRevenueSource> GetRevenueHeadwiseGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoRevenueSource>("dbo.sp_GetRevenueSourceGraph", new { Year = objPropertyFilter.Year, ProperyTypeId = objPropertyFilter.PropertyTypeId, Sector = objPropertyFilter.Sector, block = objPropertyFilter.Block }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoComplaintGraph>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyComplaintStatusGraph }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<DtoComplaintGraph> GetPropertyComplaintGraph1(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                //return connection.BeginTransaction
                return connection.Query<DtoComplaintGraph>(storedProcedure, new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, ReportFlag = Contants.GraphType.PropertyLegalGraph }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        #endregion
    }
}
