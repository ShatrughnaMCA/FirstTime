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
    
    public partial class RECEIPT_SUB_HEAD
    {
        public RECEIPT_SUB_HEAD()
        {
            this.Building_Plan_Trans = new HashSet<Building_Plan_Trans>();
            this.Property_Ledger = new HashSet<Property_Ledger>();
            this.RECEIPT_AMOUNT_TRANS = new HashSet<RECEIPT_AMOUNT_TRANS>();
        }
    
        public int RECEIPT_SUBHEAD_ID { get; set; }
        public string RECEIPT_SUB_HEAD1 { get; set; }
        public Nullable<int> RECEIPT_CODE { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string USERID { get; set; }
        public Nullable<System.DateTime> ENTRY_DATE { get; set; }
    
        public virtual ICollection<Building_Plan_Trans> Building_Plan_Trans { get; set; }
        public virtual ICollection<Property_Ledger> Property_Ledger { get; set; }
        public virtual ICollection<RECEIPT_AMOUNT_TRANS> RECEIPT_AMOUNT_TRANS { get; set; }
        public virtual RECIEPT_HEAD RECIEPT_HEAD { get; set; }
    }
}