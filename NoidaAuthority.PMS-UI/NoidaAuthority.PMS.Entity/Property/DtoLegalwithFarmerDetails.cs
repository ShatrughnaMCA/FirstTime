using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoLegalwithFarmerDetails
    {
        public double? SLNO { get; set; }
          public string Rid_no { get; set; }
          public string CASENO { get; set; }
          public double? YEAR { get; set; }
          public string COURT { get; set; }
          public string PARTIES { get; set; }
          public string DEPTT { get; set; }
          public string Address { get; set; }
          public string DESCRIPTION { get; set; }
          public string ADVOCATEDETAILS { get; set; }
          public string STAY { get; set; }
          public string FiledFlag { get; set; }
          public string DATE { get; set; }
          public string NARRATIVEYES_NO { get; set; }
    }
}
