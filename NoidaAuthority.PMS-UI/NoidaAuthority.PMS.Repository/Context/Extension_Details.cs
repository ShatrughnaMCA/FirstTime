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
    
    public partial class Extension_Details
    {
        public int Id { get; set; }
        public Nullable<int> Rid { get; set; }
        public string Property_No { get; set; }
        public Nullable<System.DateTime> Completion_DueDate { get; set; }
        public Nullable<System.DateTime> Extension_Given_Date { get; set; }
        public Nullable<System.DateTime> Extension_Due_Date { get; set; }
        public Nullable<decimal> Extension_Charge { get; set; }
        public Nullable<int> Approved_By { get; set; }
        public Nullable<System.DateTime> Approved_Date { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string RComment { get; set; }
        public Nullable<System.DateTime> RCommentDate { get; set; }
        public Nullable<int> OnlineRequestNo { get; set; }
    
        public virtual AllotmentMaster AllotmentMaster { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
    }
}