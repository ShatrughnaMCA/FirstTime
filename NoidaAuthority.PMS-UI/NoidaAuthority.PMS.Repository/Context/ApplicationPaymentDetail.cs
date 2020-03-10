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
    
    public partial class ApplicationPaymentDetail
    {
        public int id { get; set; }
        public Nullable<int> applicationId { get; set; }
        public string formNo { get; set; }
        public string paymentMode { get; set; }
        public Nullable<int> bankId { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<decimal> amountDeposited { get; set; }
        public string ddNo { get; set; }
        public string ddIssueBank { get; set; }
        public Nullable<System.DateTime> ddIssueDate { get; set; }
        public string utn { get; set; }
        public Nullable<int> challanNo { get; set; }
        public Nullable<System.DateTime> challanIssueDate { get; set; }
        public Nullable<System.DateTime> paymentDate { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual ApplicationDetail ApplicationDetail { get; set; }
        public virtual BankMst BankMst { get; set; }
        public virtual BranchMst BranchMst { get; set; }
    }
}
