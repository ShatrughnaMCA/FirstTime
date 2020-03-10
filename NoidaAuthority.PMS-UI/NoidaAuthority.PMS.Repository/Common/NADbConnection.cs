using NoidaAuthority.PMS.Common.Helpers;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Repository
{
    public class NADbConnection
    {
        public static DbConnection GetPISConnection()
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PISSqlConnection"].ConnectionString);
            if (SettingsHelper.IsMiniProfilerEnabled)
            {
                return new StackExchange.Profiling.Data.ProfiledDbConnection(conn, MiniProfiler.Current);
            }
            else
            {
                return conn;
            }
        }

        public static DbConnection GetPIMSConnection()
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PIMSSqlConnection"].ConnectionString);
            if (SettingsHelper.IsMiniProfilerEnabled)
            {
                return new StackExchange.Profiling.Data.ProfiledDbConnection(conn, MiniProfiler.Current);
            }
            else
            {
                return conn;
            }
        }
    }
}
