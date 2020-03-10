using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoPropertyPaymentHistory
    {
        public string Account { get; set; }
        public string ScheduledDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int AmountDue { get; set; }
        public double? AmountPaid { get; set; }
        public string PaymentMode { get; set; }
        public string BankId { get; set; }
        public string ChallanId { get; set; }
        public string Status { get; set; }
        public string PropertyNo { get; set; }
        public string PropId { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string AlloteName { get; set; }
        public string DepositorName { get; set; }
        public DateTime? DepositDate { get; set; }
        public int PropertyId  { get; set; }
        public int RegistrationId { get; set; }
        public string PropertyType { get; set; }
        public string ReceiptHead { get; set; }

       
       
    }
}
