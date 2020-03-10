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
    
    public partial class RECIEPT_HEAD
    {
        public RECIEPT_HEAD()
        {
            this.Property_Ledger = new HashSet<Property_Ledger>();
            this.RECEIPT_AMOUNT_TRANS = new HashSet<RECEIPT_AMOUNT_TRANS>();
            this.RECEIPT_SUB_HEAD = new HashSet<RECEIPT_SUB_HEAD>();
        }
    
        public int RECIEPT_CODE { get; set; }
        public string RECIEPT_HEAD_NAME { get; set; }
        public Nullable<int> HEAD_CODE { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string USERID { get; set; }
        public Nullable<System.DateTime> ENTRY_DATE { get; set; }
    
        public virtual HEAD_MASTER HEAD_MASTER { get; set; }
        public virtual ICollection<Property_Ledger> Property_Ledger { get; set; }
        public virtual ICollection<RECEIPT_AMOUNT_TRANS> RECEIPT_AMOUNT_TRANS { get; set; }
        public virtual ICollection<RECEIPT_SUB_HEAD> RECEIPT_SUB_HEAD { get; set; }
    }
}
