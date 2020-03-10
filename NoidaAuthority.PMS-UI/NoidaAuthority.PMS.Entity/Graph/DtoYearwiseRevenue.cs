using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoYearwiseRevenue : BaseEntity
    {
        public string Year { get; set; }
        public string PropertyType { get; set; }
        public double Revenue { get; set; }

    }
}
