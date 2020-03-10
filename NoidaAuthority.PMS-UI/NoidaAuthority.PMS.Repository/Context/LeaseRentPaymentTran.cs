//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoidaAuthority.PMS.Repository.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class LeaseRentPaymentTran
    {
        public int Id { get; set; }
        public Nullable<int> RefId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<decimal> LeaseRentPremium { get; set; }
        public Nullable<decimal> NormalInterest { get; set; }
        public Nullable<decimal> PenalInterest { get; set; }
        public Nullable<decimal> GST { get; set; }
        public Nullable<decimal> DuesAmount { get; set; }
        public Nullable<System.DateTime> DuesUptoDate { get; set; }
        public Nullable<System.DateTime> DepositDueDate { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public string HtmlTemplate { get; set; }
        public string Comment { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> RequestRefNo { get; set; }
    
        public virtual LeaseRentPayment LeaseRentPayment { get; set; }
    }
}