using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoPaymentSchedule
    {
        public int PropertyId { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PropertyNo { get; set; }
        public string ApplicantName { get; set; }
        public DateTime? AllotmentDate { get; set; }
        public DateTime? paymentScheduleDate { get; set; }
        public double? ScheduleAmount { get; set; }
        public int? RegistrationId { get; set; }
        public string PaymentStatus { get; set; }
        public int PaymentScheduleId { get; set; }
        public int PendingDueAmount { get; set; }
    }
    public class DtoPaymentDetails
    {
        public string  RECEIPT_ID { get; set; }
        public string ChallanId { get; set; }
        public DateTime DepositDate { get; set; }
        public double AmountPaid { get; set; }
        public string ReceiptHead { get; set; }
    }
}
