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
    
    public partial class PaymentTypeMst
    {
        public PaymentTypeMst()
        {
            this.PaymentSubTypeMsts = new HashSet<PaymentSubTypeMst>();
        }
    
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual ICollection<PaymentSubTypeMst> PaymentSubTypeMsts { get; set; }
    }
}
