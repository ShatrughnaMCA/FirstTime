using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Common.Helpers
{
    public static class SettingsHelper
    {
        public static bool IsMiniProfilerEnabled
        {
            get
            {
                string result = ConfigurationManager.AppSettings["IsMiniProfilerEnabled"];
                return Convert.ToBoolean(result);
            }
        }
    }
}
