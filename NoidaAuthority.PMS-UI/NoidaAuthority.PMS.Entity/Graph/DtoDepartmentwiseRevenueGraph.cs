using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoDepartmentwiseRevenueGraph : BaseEntity
    {
        public string Monyear { get; set; }
        public int? defaultedNumber { get; set; }
        public double? DefaultedAmount { get; set; }
        public string propertyType { get; set; }
    }
}
