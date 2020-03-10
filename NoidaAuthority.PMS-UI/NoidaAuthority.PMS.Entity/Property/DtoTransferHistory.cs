using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoTransferHistory
    {
        public int PropertyId { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PropertyNo { get; set; }
        public DateTime? MutationDate { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantFatherName { get; set; }
        public double? SellingCost { get; set; }
        public double? Amount { get; set; }
        public DateTime? AllotmentDate { get; set; }
        public DateTime? TransferdeedDate { get; set; }
        public int? RegistrationId { get; set; }
        public string OwnerName { get; set; }
        public string TransfereeName { get; set; }
    }
}
