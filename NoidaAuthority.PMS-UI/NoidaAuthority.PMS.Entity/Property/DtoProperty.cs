using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoProperty : BaseEntity
    {
        public double? PropertyId { get; set; }
        public string PropertyNo { get; set; }
        public string Block { get; set; }
        public string Sector { get; set; }
        public string ApplicantName { get; set; }
        public string PropertyType { get; set; }
        public string Area { get; set; }
        public int? RegistrationId { get; set; }
        public string PlotNumber { get; set; }
        public int TotalCount { get; set; }
        public string RID { get; set; }
        public double? AreaSqm { get; set; }
    }


}
