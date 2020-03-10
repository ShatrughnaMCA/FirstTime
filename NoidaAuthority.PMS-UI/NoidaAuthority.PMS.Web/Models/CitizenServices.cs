using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoidaAuthority.PMS.Web.Models
{
    public class MortgageModel
    {
        public int RegistrationID { get; set; }
        public Nullable<System.DateTime> MortgageDate { get; set; }
        public Nullable<decimal> SanctionedAmount { get; set; }
        public string BankName { get; set; }
        public string BranchAddress { get; set; }
        public string MortgageType { get; set; }
        public Nullable<System.DateTime> ValidUpto { get; set; }
        public Nullable<decimal> ProcessingFee { get; set; }
        public Nullable<short> PreviousLoanNoc { get; set; }
    }
}