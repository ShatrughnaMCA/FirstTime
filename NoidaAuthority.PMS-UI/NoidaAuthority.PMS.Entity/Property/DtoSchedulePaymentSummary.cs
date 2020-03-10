using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoSchedulePaymentSummary
    {
        public double TotalDueAmount { get; set; }
        public int TotalEMICount { get; set; }
        public double TotalPaidAmount { get; set; }
    }
}
