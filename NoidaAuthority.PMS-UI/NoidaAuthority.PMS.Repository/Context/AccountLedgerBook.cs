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
    
    public partial class AccountLedgerBook
    {
        public int Id { get; set; }
        public Nullable<int> RId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ReceiptHeadId { get; set; }
        public Nullable<int> ReceiptSubHeadId { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public Nullable<System.DateTime> CreditDate { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<System.DateTime> DebitDate { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Comment { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
