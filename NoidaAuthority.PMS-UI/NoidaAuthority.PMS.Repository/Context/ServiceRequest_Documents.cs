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
    
    public partial class ServiceRequest_Documents
    {
        public int SrvDocId { get; set; }
        public int ServiceReq_No { get; set; }
        public Nullable<int> ChkId { get; set; }
        public string DocumentPath { get; set; }
        public Nullable<System.DateTime> Uploaded_Date { get; set; }
        public Nullable<int> Uploaded_By { get; set; }
    
        public virtual Customer_ServiceRequest Customer_ServiceRequest { get; set; }
    }
}
