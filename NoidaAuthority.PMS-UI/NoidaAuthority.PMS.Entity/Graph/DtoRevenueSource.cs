using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoRevenueSource
    {
        public string RECIEPT_HEAD_NAME { get; set; }
        public double? Revenue { get; set; }
        public int FiscalYear { get; set; }
        public string Department { get; set; }
        public int PropertyTypeId { get; set; }
    }

}
