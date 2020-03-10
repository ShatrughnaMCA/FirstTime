using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoDepartmentPaymentDetails
    {
        public int RegistrationId { get; set; }
        public string PropertyNo { get; set; }
        public string SECTOR { get; set; }
        public string Department { get; set; }
        public string Account { get; set; }
        public double? AmountDue { get; set; }
        public double? AmountPaid { get; set; }
        public DateTime PaymentAson { get; set; }
        public string ApplicantName { get; set; }
    }
}
